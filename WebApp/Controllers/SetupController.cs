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
            ViewBag.width = HttpContext.Session.GetInt32("width");
            ViewBag.height = HttpContext.Session.GetInt32("height");

            // TODO: #36 get rid of / phase out with grid - store W/H in session, store live cells
            World? world = GetWorld();
            if(world == null) { return RedirectToAction("Index", "Home"); }
            ShowWorld(world);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult ToggleCell([FromBody] CellToggle info)
        {
            World? world = GetWorld();
            if (world == null) { return RedirectToAction("Index", "Home"); }

            int i = info.i;
            int j = info.j;
            if (world.IsInGrid(i, j)){ world.ToggleCell(i, j); }
            else { ViewBag.alert = "outOfGrid"; }

            ShowWorld(world);
            // returns not View("Index") but JSON for Javascript fetch call
            JsonResult ret = Json(new {message = ViewBag.Message});
            return ret;
        }        

        [HttpPost]
        public IActionResult RunGame()
        {
            // proceed to game
            return RedirectToAction("Index", "Launch");
        }

        // TODO: #20 base controller
        private void ShowWorld(World world)
        {
            string world_text = TextAdapter.GetWorldString(world);
            HttpContext.Session.SetString("world", world_text);
            ViewBag.Message = $"\n{world_text}";
        }

        private World? GetWorld()
        {
            string? world_repr = HttpContext.Session.GetString("world");
            if (world_repr == null) return null;

            World world = TextAdapter.GetWorldFromString(world_repr);
            return world;
        }
    }
}
