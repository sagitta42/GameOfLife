using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    internal class Program
    {
        private static int cycle_time = 1500;
        static void Main(string[] args)
        {
            ConsoleManager console_manager = new ConsoleManager();
            InputManager input_manager = new InputManager(console_manager);

            bool keep_playing = true;
            while (keep_playing)
            {
                RunGame(console_manager, input_manager);
                keep_playing = input_manager.PlayAgain();
            }
            console_manager.WriteLine("Bye!", sleep_time: cycle_time);
        }

        static void RunGame(ConsoleManager console_manager, InputManager input_manager)
        {
            (int, int) size = input_manager.GetWorldSize();
            World world = new World(size);
            console_manager.Show(world);

            input_manager.SetInitLiveCells(world);

            console_manager.Show(world, sleep_time: cycle_time);
            bool run_flag = true;
            while (run_flag)
            {
                world.Cycle();
                console_manager.Show(world, sleep_time: cycle_time, overwrite: true);

                // TODO: #17 detect oscillator and stop program
                if (world.is_stable)
                {
                    console_manager.WriteLine("STABLE", sleep_time: cycle_time);
                    run_flag = false;
                }
                if (!world.is_populated)
                {
                    console_manager.WriteLine("THE END", sleep_time: cycle_time);
                    run_flag = false;
                }
            }

        }
    }
}
