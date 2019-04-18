using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderAPI;
using OrderAPI.Data;
using OrderAPI.Infrastructure.Filters;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using MySql.Data.MySqlClient;
using System.Threading;

namespace OrderAPI
{
    public class Startup
    {
        ILogger _logger;
        public IConfiguration _config { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _logger = loggerFactory.CreateLogger<Startup>();
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(
                 options => options.Filters.Add(typeof(HttpGlobalExceptionFilter))
                 )
                  .AddJsonFormatters(
                   Options =>
                   {
                       Options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                       Options.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   }
                )
                  .AddApiExplorer();
 //           services.Configure<OrderSettings>(Configuration);

            ConfigureAuthService(services);

            // WaitForDBInit(_connectionString);

            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "mssqlserver";
            var password = Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "MyProduct!123";
            var database = Environment.GetEnvironmentVariable("DATABASE") ?? "OrdersDb";
            var connectionString = $"Server={hostname};Database={database};User ID=sa;Password={password};";

            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(connectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Ordering HTTP API",
                    Version = "v1",
                    Description = "The Ordering Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{_config["IdentityUrl"]}/connect/authorize",
                    TokenUrl = $"{_config["IdentityUrl"]}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "order", "Order Api" }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    poly => poly.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            var builder = new ContainerBuilder();//create a container/bus for a message
            builder.Register(c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(rmq =>
                {
                    //port for rabbitmq
                    rmq.Host(new Uri("rabbitmq://rabbitmq"), "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    rmq.ExchangeType = ExchangeType.Fanout;//can fanout to multiple receipients
                });

            }).
             As<IBusControl>()
            .As<IBus>()
            .As<IPublishEndpoint>() //make it as publishendpoint, I am a publisher.
            .SingleInstance(); //singleton

            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = _config["IdentityUrl"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "order";

            });
        }
        private void WaitForDBInit(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            int retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine("Connecting to db. Trial: {0}", retries);
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch (MySqlException)
                {
                    Thread.Sleep((int)Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var pathBase = _config["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }


            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "OrderApi V1");
                  //c.ConfigureOAuth2("orderswaggerui", "", "", "Ordering Swagger UI");
              });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });

        }
    }
}
