using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMvc.services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;

        public CatalogController(ICatalogService service) =>
            _service = service;

        public async Task<IActionResult> Index(int? categoryFilterApplied, int? metroAreaFilterApplied, int? page)
        {
            var itemsOnPage = 10;

            var catalog =  await _service.GetEventsAsync(page ?? 0, itemsOnPage, categoryFilterApplied, metroAreaFilterApplied);

            var vm = new CatalogIndexViewModel
            {
                Events = catalog.Data,
                Categories = await _service.GetCategoriesAsync(),
                MetroAreas = await _service.GetMetroAreasAsync(),
                CategoryFilterApplied = categoryFilterApplied ?? 0,
                MetroAreaFilterApplied = metroAreaFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                }
            };

            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";

            return View(vm);
        }
    }
}