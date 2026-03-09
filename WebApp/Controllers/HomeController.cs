using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

using GameLogic;
using GameAdapter;
using Utils;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] header_lines = Utils.Utils.GetHeaderLines();
            string header = "";
            foreach (string line in header_lines) { header += "\n" + line; }
            ViewBag.header = header;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult CreateWorld(int width, int height)
        {
            HttpContext.Session.SetInt32("width", width);
            HttpContext.Session.SetInt32("height", height);

            // TODO: #26 get rid of this - get world from W/H + later stored live cells
            World world = new World(width, height);
            string world_text = TextAdapter.GetWorldString(world);

            // proceed to cell setup
            HttpContext.Session.SetString("world", world_text);            
            return RedirectToAction("Index", "Setup");
        }        
    }
}
