using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    internal class Program
    {
        static void Main(string[] args)
        {
            World world = new World(3,3);

            world.Toggle(0, 1);
            world.Toggle(1, 1);
            world.Toggle(2, 1);

            int cycle_time = 1500;
            int n_cycles = 12;

            for (int i = 0; i < n_cycles; i++)
            {
                world.Show();
                world.Cycle();
                System.Threading.Thread.Sleep(cycle_time);
            }
        }
    }
}
