using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MassTransit;
using OrderAPI.Data;
using OrderAPI.Messaging;
using OrderAPI.Model;

namespace OrderAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {

        private readonly OrderContext _orderContext;
        private readonly IConfiguration _config; 
    //    private readonly IOptionsSnapshot<OrderSettings> _settings;


        private readonly ILogger<OrdersController> _logger;
        private IBus _bus;

        public OrdersController(OrderContext orderContext, ILogger<OrdersController> logger, IConfiguration config, IBus bus)
        {
            _config = config;
            // _orderContext = ordersContext;
            _orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));

            ((DbContext)orderContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _bus = bus;
            _logger = logger;
        }

        // GET api/values


        // POST api/v1/Orders/new
        [Route("new")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var envs = Environment.GetEnvironmentVariables();
            var conString = _config["ConnectionString"];
            _logger.LogInformation($"{conString}");

            order.OrderStatus = OrderStatus.Preparing;
            order.OrderDate = DateTime.UtcNow;

            _logger.LogInformation(" In Create Order");
            _logger.LogInformation(" Order" + order.UserName);

            _orderContext.Orders.Add(order);
            _orderContext.OrderItems.AddRange(order.OrderItems);

            _logger.LogInformation(" Order added to context");
            _logger.LogInformation(" Saving........");
            try
            {
                await _orderContext.SaveChangesAsync();
                _logger.LogWarning("BuyerId is: " + order.BuyerId);
                _bus.Publish(new OrderCompletedEvent(order.BuyerId)).Wait();
                return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("An error occored during Order saving .." + ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "GetOrder")]
        //  [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrder(int id)
        {
            var item = await _orderContext.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefaultAsync(ci => ci.OrderId == id);
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderContext.Orders.ToListAsync();
            return Ok(orders);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
