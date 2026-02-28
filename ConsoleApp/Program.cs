using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using GameLogic;
using GameFlow;
using Utils;

namespace ConsoleApp
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
                (int, int) size = input_manager.GetWorldSize();
                World world = new World(size);
                console_manager.Show(world);
                input_manager.SetInitLiveCells(world);

                Run.RunGame(world, console_manager, cycle_time);
                keep_playing = input_manager.PlayAgain();
            }
            console_manager.WriteLine("Bye!", sleep_time: cycle_time);
        }

        private static void ShowHeader(ConsoleManager console_manager)
        {
            string[] header_lines = Utils.Utils.GetHeaderLines();
            foreach(string line in header_lines)
            {
                console_manager.WriteLine(line);
            }
        }
    }
}

