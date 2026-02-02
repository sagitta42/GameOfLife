using GameOfLife;

namespace UnitTests
{
    public class GridTest
    {
        [Fact]
        public void BasicTest()
        {
            World world = new World(3, 3);
            int cycle_time = 800;

            for (int i = 0; i < world.length; i++)
            {
                for (int j = 0; j < world.height; j++)
                {
                    world.Toggle(i, j);
                    world.Show(overwrite: false);
                    System.Threading.Thread.Sleep(cycle_time);
                }
            }
        }
    }
}