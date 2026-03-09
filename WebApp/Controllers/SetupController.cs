using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

using GameLogic;
using GameAdapter;
using GameOfLife.Models;

namespace WebApp.Controllers
{
    public class SetupController : Controller
    {
        private readonly ILogger<SetupController> _logger;

        public SetupController(ILogger<SetupController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int? width = HttpContext.Session.GetInt32("width");
            int? height = HttpContext.Session.GetInt32("height");
            if(width == null || height == null) { return RedirectToAction("Index", "Home"); }

            ViewBag.width = width;
            ViewBag.height = height;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SaveSetup([FromBody] Grid grid)
        {
            int? width = HttpContext.Session.GetInt32("width");
            int? height = HttpContext.Session.GetInt32("height");            
            if(width == null || height == null) { return RedirectToAction("Index", "Home"); }
            World world = new World((int) width, (int)height);

            for (int i = 0; i < grid.GetLength(); i++)
            {
                for (int j = 0; j < grid.GetHeight(); j++)
                {
                    if (grid.IsAlive(i, j)) { world.ToggleCell(i, j); }
                }
            }

            // TODO: #26 pass/set grid directly to Launch; or better - stay and run in this view
            string world_text = TextAdapter.GetWorldString(world);
            HttpContext.Session.SetString("world", world_text);
            return Json(new {});
        }

        [HttpPost]
        public IActionResult RunGame()
        {
            return RedirectToAction("Index", "Launch");
        }
    }
}
