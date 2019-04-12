using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public class ApiPaths
    {
        public static class Catalog
        {
            public static string GetAllEvents(string baseUri, int page, int take, int? category, int? metroArea)
            {
                var filterQs = String.Empty;

                if (category.HasValue || metroArea.HasValue)
                {
                    var categoryQs = (category.HasValue) ? category.ToString() : "null";
                    var metroAreaQs = (metroArea.HasValue) ? metroArea.ToString() : "null";

                    filterQs = $"/category/{categoryQs}/metroArea/{metroAreaQs}";
                }

                return $"{baseUri}events{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetAllCategories(string baseUri)
            {
                return $"{baseUri}eventcategories";
            }

            public static string GetAllMetroAreas(string baseUri)
            {
                return $"{baseUri}eventmetroareas";

            }
        }
    }
}
