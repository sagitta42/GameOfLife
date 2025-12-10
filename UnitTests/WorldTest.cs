using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;

namespace GameOfLife.Tests
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void GridTest()
        {
            World world = new World(3, 3);

            for (int i = 0; i < world.length; i++)
            {
                for (int j = 0; j < world.height; j++)
                {
                    Assert.IsTrue(world.GetCell(i, j) != null);
                }
            }
        }
    }
}
