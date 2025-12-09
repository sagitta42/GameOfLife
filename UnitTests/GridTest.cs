using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;

namespace GameOfLife.Tests
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void BasicTest()
        {
            ConsoleManager console_manager = new ConsoleManager();

            World world = new World(3, 3);
            int cycle_time = 800;

            for (int i = 0; i < world.length; i++)
            {
                for (int j = 0; j < world.height; j++)
                {
                    world.ToggleCell(i, j);
                    console_manager.Show(world, cycle_time: cycle_time);
                }
            }
        }
    }
}
