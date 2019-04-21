using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly string _remoteServiceBaseUri;  //=use for IIS  "http://localhost:39292/api/catalog/";

        private readonly IHttpClient _client;

        public CatalogService(IHttpClient client, IConfiguration configuration)
        {
            _client = client;
            _remoteServiceBaseUri = $"{configuration["CatalogUrl"]}/api/catalog/";
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            var categoryUri = ApiPaths.Catalog.GetAllCategories(_remoteServiceBaseUri);
            var dataString = await _client.GetStringAsync(categoryUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var categories = JArray.Parse(dataString);

            foreach (var category in categories)
            {
                items.Add
                    (
                    new SelectListItem
                    {
                        Value= category.Value<string>("id"),
                        Text = category.Value<string>("category")
                    }
                    );
            }
            return items;
        }

        public async Task<Catalog> GetEventsAsync(int page, int take, int? category, int? metroArea)
        {

            var eventsUri = ApiPaths.Catalog.GetAllEvents(_remoteServiceBaseUri,page,take,category,metroArea);
            var dataString = await _client.GetStringAsync(eventsUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetMetroAreasAsync()
        {
            var metroAreaUri = ApiPaths.Catalog.GetAllMetroAreas(_remoteServiceBaseUri);
            var dataString = await _client.GetStringAsync(metroAreaUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var metroAreas = JArray.Parse(dataString);

            foreach (var metroArea in metroAreas)
            {
                items.Add
                    (
                    new SelectListItem
                    {
                        Value = metroArea.Value<string>("id"),
                        Text = metroArea.Value<string>("metroArea")
                    }
                    );
            }
            return items;
        }
    }
}
