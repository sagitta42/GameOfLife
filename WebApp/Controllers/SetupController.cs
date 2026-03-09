using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

using GameLogic;
using GameAdapter;

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
        public IActionResult ToggleCell(int x, int y)
        {
            World? world = GetWorld();
            if (world == null) { return RedirectToAction("Index", "Home"); }

            if (world.IsInGrid(x, y)){ world.ToggleCell(x, y); }
            else { ViewBag.alert = "outOfGrid"; }

            ShowWorld(world);
            return View("Index");
        }

        [HttpPost]
        public ActionResult ToggleCheckbox(int x, int y)
        {
            // TODO: #26 unite/replace with ToggleCell (WIP)
            World? world = GetWorld();
            if (world == null) { return RedirectToAction("Index", "Home"); }

            if (world.IsInGrid(x, y)){ world.ToggleCell(x, y); }
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
