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
            string? world_repr = HttpContext.Session.GetString("world");
            if (world_repr == null) return RedirectToAction("Index", "Home");

            World world = TextAdapter.GetWorldFromString(world_repr);

            string world_text = TextAdapter.GetWorldString(world);
            ViewBag.Message = $"\n{world_text}";

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
            string? world_repr = HttpContext.Session.GetString("world");
            if (world_repr == null) return RedirectToAction("Index", "Setup");

            World world = TextAdapter.GetWorldFromString(world_repr);
            world.ToggleCell(x, y);

            string world_text = TextAdapter.GetWorldString(world);
            ViewBag.Message = $"\n{world_text}";
            return View("Index");
        }
    }
}
