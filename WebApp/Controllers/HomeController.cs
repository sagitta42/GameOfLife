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
            // TODO: #26 pass to controller index directly, not through session
            HttpContext.Session.SetInt32("width", width);
            HttpContext.Session.SetInt32("height", height);

            return RedirectToAction("Index", "Game");
        }        
    }
}
