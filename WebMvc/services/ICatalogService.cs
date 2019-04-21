using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public interface ICatalogService
    {
        //These data come from microservices
        Task<Catalog> GetEventsAsync(int page, int take, int? category, int? metroArea);

        Task<IEnumerable<SelectListItem>> GetCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetMetroAreasAsync();
    }
}
