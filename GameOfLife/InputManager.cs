using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class InputManager
    {
        private ConsoleManager console_manager;
        private static char grid_char = 'x';
        private static char coord_char = ',';
        private static char run_char = 'R';

        private static string message_intro_world = "Enter world size in format NxM: ";
        private static string message_intro_config = "Enter coordinates of cell to toggle in X,Y format; enter ? if ready to run: ";

        private static string error_format_separator = "Please use N?M format (example: 5?5): ";
        private static string error_format_number = "Please use integers for N and M (N?M): ";
        private static string error_out_of_grid = "Coordinate out of grid!";

        public InputManager(ConsoleManager consoleOutput) { console_manager = consoleOutput; }
        
        public (int, int) GetWorldSize()
        {
            (int, int) ret = GetCoordinatesFromInput(message_intro_world, grid_char);
            return ret.Item1 == -1 ? GetWorldSize() : ret;
        }
        public void SetInitLiveCells(World world)
        {
            bool run_flag = false;
            (int, int) coord;
            while (!run_flag)
            {
                coord = GetCoordinatesFromInput(message_intro_config.Replace('?', run_char), coord_char);
                if(coord.Item1 == -1)
                {
                    run_flag = true;
                }
                else
                {
                    if (world.IsInGrid(coord))
                    {
                        // TODO: #7 implement toggling multiple cells based on keyword common structure
                        world.ToggleCell(coord);
                        console_manager.Show(world, overwrite: true);
                    }
                    else
                    {
                        console_manager.WriteLine(error_out_of_grid);
                    }
                }
            }
        }
        public (int, int) GetCoordinatesFromInput(string message, char separator)
        {
            console_manager.Write(message.Replace('?', separator));
            string user_input = console_manager.ReadLine();

            if (user_input == $"{run_char}") { return (-1, -1); }

            if (!user_input.Contains(separator)) { return GetCoordinatesFromInput(error_format_separator, separator); }

            string[] parts = user_input.Split(separator);
            int[] ret = new int[2];
            for (int i = 0; i < 2; i++)
            {
                int size;
                if (int.TryParse(parts[i], out size))
                {
                    ret[i] = size;
                }
                else
                {
                    return GetCoordinatesFromInput(error_format_number, separator);
                }
            }
            return (ret[0], ret[1]);
        }
    }
}
