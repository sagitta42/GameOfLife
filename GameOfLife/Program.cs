using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace GameOfLife
{
    internal class Program
    {
        private static int cycle_time = 1500;
        static void Main(string[] args)
        {
            ConsoleManager console_manager = new ConsoleManager();
            ShowHeader(console_manager);

            InputManager input_manager = new InputManager(console_manager);

            bool keep_playing = true;
            while (keep_playing)
            {
                RunGame(console_manager, input_manager);
                keep_playing = input_manager.PlayAgain();
            }
            console_manager.WriteLine("Bye!", sleep_time: cycle_time);
        }

        private static void RunGame(ConsoleManager console_manager, InputManager input_manager)
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

        private static void ShowHeader(ConsoleManager console_manager)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string v = $"v{version.Major}.{version.Minor}.{version.MinorRevision}";
            string header = "|o GameOfLife " + v + " o|";

            string frame = new string('-', header.Length - 2);
            frame = " " + frame + " ";
            string subframe = "";
            string cell;
            for (int i = 0; i < header.Length - 2; i++)
            {
                cell = i % 2 == 0 ? " " : "o";
                subframe = subframe + cell;
            }
            subframe = "|" + subframe + "|";

            console_manager.WriteLine(frame);
            console_manager.WriteLine(subframe);
            console_manager.WriteLine(header);
            console_manager.WriteLine(subframe);
            console_manager.WriteLine(frame);
        }
    }
}

