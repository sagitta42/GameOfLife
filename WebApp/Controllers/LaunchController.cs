using System.Diagnostics;
using GameAdapter;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LaunchController : Controller
    {
        private readonly ILogger<SetupController> _logger;

        public LaunchController(ILogger<SetupController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // TODO: #20 can recycle? controller inheritance?.. interface?
            World? world = GetWorld();
            if (world == null) { return RedirectToAction("Index", "Home"); }
            ShowWorld(world);
            ViewBag.status = "start";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult RunCycle()
        {
            // TODO: #20 runs single cycle at each click - implement auto-polling

            // FIXME: #20 figure out if possible to manage to use Game Flow without needing to replicate steps here
            // (difficulty - ViewBag update each cycle)
            //Run.RunGame(world, _gameInterface);

            World? world = GetWorld();
            if (world == null) { return RedirectToAction("Index", "Home"); }

            world.Cycle();
            ShowWorld(world);

            if (world.is_stable)
            {
                ViewBag.Message = ViewBag.Message + "\n" + "STABLE";
                ViewBag.status = "end";
            }
            if (!world.is_populated)
            {
                ViewBag.Message = ViewBag.Message + "\n" + "THE END";
                ViewBag.status = "end";
            }

            return View("Index");
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
