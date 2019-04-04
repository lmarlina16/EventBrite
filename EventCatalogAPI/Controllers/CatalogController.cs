using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogAPI.Data;
using EventCatalogAPI.Domain;
using EventCatalogAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventCatalogAPI.Controllers
{
    //

    [Produces("application/json")]
    [Route("api/[controller]")]

    public class CatalogController : Controller
    {

        private readonly EventContext _context;
        private readonly IConfiguration _config;
        public CatalogController(EventContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Route("events/{id:int}")]
        // http://localhost:39292/api/Catalog/events/5
        public async Task<IActionResult> GetEventsById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect Id!");
            }

            var events = await _context.Events.SingleOrDefaultAsync(c => c.Id == id);
            events.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced"
                    , _config["ExternalCatalogBaseUrl"]);
            return Ok(events);
        }

        [HttpGet]
        [Route("events/zip/{zipcode:int}")]
        // http://localhost:39292/api/Catalog/events/zip/94102
        public IActionResult GetEventsByZip(int zipcode)
        {
            var events = _context.Events.Where(c => c.ZipCode == zipcode).ToList();
            events = ChangePictureUrl(events);
            return Ok(events);
        }

        [HttpGet]
        [Route("[action]")]
        // http://localhost:39292/api/Catalog/EventsCategory
        public async Task<IActionResult> EventsCategory()
        {
            List<EventCategory> events = await _context.EventCategories.ToListAsync();
            return Ok(events);
        }

        [HttpGet]
        [Route("events/{date:DateTime}")]
        // http://localhost:39292/api/Catalog/events/4-30-2019
        public IActionResult GetEventsByDate(DateTime date)
        {
            var events = _context.Events.Where(c => 
            c.Date.Year == date.Year &&
            c.Date.Month == date.Month &&
            c.Date.Day == date.Day
            ).ToList();
            events = ChangePictureUrl(events);
            return Ok(events);
        }

        // http://localhost:39292/api/Catalog/Events?pageIndex=1&pageSize=4   <-- paginated
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Events(
               [FromQuery] int pageSize = 6,
               [FromQuery] int pageIndex = 0)
        {
            var eventsCount =
                await _context.Events.LongCountAsync();

            var events = await _context.Events
                .OrderBy(c => c.Title)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            events = ChangePictureUrl(events);

            var model = new PaginatedItemsViewModel<Event>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = eventsCount,
                Data = events
            };
            return Ok(model);

        }

        private List<Event> ChangePictureUrl(List<Event> items)
        {
            items.ForEach(
                c => c.PictureUrl = c.PictureUrl
                    .Replace("http://externalcatalogbaseurltobereplaced"
                    , _config["ExternalCatalogBaseUrl"])
                );
            return items;
        }

    }
}