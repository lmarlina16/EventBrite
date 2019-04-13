using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalogAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PicController : Controller
    {
        private readonly IHostingEnvironment _env;
        public PicController(IHostingEnvironment env)
        {
            _env = env;
        }

        // http://localhost:5510/api/pic/5
        // GET api/pic/5
        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Pics/", "event-" + id + ".jpg");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/jpg");
        }

    }
}
