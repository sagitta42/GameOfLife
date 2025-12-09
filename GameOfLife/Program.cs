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
            ConsoleManager console_manager = new ConsoleManager();

            InputManager input_manager = new InputManager(console_manager);

            (int, int) size = input_manager.GetWorldSize();
            World world = new World(size);
            console_manager.Show(world);
            
            input_manager.SetInitLiveCells(world);

            int cycle_time = 1500;
            console_manager.Show(world, cycle_time: cycle_time);
            bool run_flag = true;
            while(run_flag)
            {
                world.Cycle();
                console_manager.Show(world, cycle_time: cycle_time, overwrite: true);

                // TODO: #17 detect oscillator and stop program
                if (world.is_stable)
                {
                    console_manager.WriteLine("STABLE");
                    run_flag = false;
                }
                if (!world.is_populated)
                {
                    console_manager.WriteLine("THE END");
                    run_flag = false;
                }

            }
        }
    }
}
