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
            ViewBag.Status = "setup";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult RunCycle([FromBody] Grid grid)
        {
            // FIXME: #20 figure out if possible to use Game Flow with async
            //Run.RunGame(world, _gameInterface);

            World world = GetWorldFromGrid(grid);

            world.Cycle();

            if (world.is_stable)
            {
                ViewBag.Message = "STABLE";
                ViewBag.Status = "end";
            }
            if (!world.is_populated)
            {
                ViewBag.Message = "THE END";
                ViewBag.Status = "end";
            }

            // TODO: #26 more efficient to only send coords of cells that changed status
            return Json(new {grid = GetGridFromWorld(world).grid});
        }

        private World GetWorldFromGrid(Grid grid)
        {
            int width = grid.GetLength();
            int height = grid.GetHeight();            
            World world = new World(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (grid.IsAlive(i, j)) { world.ToggleCell(i, j); }
                }
            }
            return world;
        }

        private Grid GetGridFromWorld(World world)
        {
            // TODO: #26 move to Adapter
            Grid ret = new Grid {grid = new bool[world.height][]};
            for(int j = 0; j < world.height; j++)
            {
                ret.grid[j] = new bool[world.length];
                for(int i = 0; i < world.length; i++)
                {
                    ret.grid[j][i] = world.GetCell(i, j).IsAlive();
                }
            }
            return ret;
        }
    }
}
