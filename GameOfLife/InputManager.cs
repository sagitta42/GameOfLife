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
        private static char replace_char = '@';
        private static char yes = 'Y';
        private static char no = 'N';

        private static string message_intro_world = "Enter world size in format N@M";
        private static string message_intro_coord = "Enter coordinates of cell to toggle in X@Y format";
        private static string message_intro_run = "enter @ if ready to run";
        private static string message_play_again = "Want to play again?";

        private static string error_out_of_grid = "Coordinate out of grid!";
        private static string error_format_separator = "Please use N@M format (example: 5@5)";
        private static string error_format_number = "Please use integers for N and M (N@M)";

        public InputManager(ConsoleManager consoleOutput) {
            console_manager = consoleOutput;

            message_intro_world = message_intro_world.Replace(replace_char, grid_char);
            message_intro_coord = message_intro_coord.Replace(replace_char, coord_char);
            message_intro_run = message_intro_run.Replace(replace_char, run_char);

            message_play_again = message_play_again + $" [{yes}/{no}]";
        }
        
        public (int, int) GetWorldSize()
        {
            (int, int) ret = GetCoordinatesFromInput(message_intro_world, grid_char);
            return ret.Item1 == -1 ? GetWorldSize() : ret;
        }
        public void SetInitLiveCells(World world)
        {
            string message_intro_config = $"{message_intro_coord}; {message_intro_run}";
            bool run_flag = false;
            (int, int) coord;
            while (!run_flag)
            {
                coord = GetCoordinatesFromInput(message_intro_config, coord_char);
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

        public bool PlayAgain()
        {
            string user_input = console_manager.ReadInput(message_play_again);
            if (user_input == $"{yes}"){ return true; }
            else if (user_input == $"{no}") { return false; }
            else { return PlayAgain(); }
        }

        private (int, int) GetCoordinatesFromInput(string message, char separator)
        {
            string user_input = console_manager.ReadInput(message.Replace(replace_char, separator));

            if (user_input == $"{run_char}") { return (-1, -1); }

            if (!user_input.Contains(separator)) {
                return GetCoordinatesFromInput(error_format_separator.Replace(replace_char, separator), separator);
            }

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
                    return GetCoordinatesFromInput(error_format_number.Replace(replace_char, separator), separator);
                }
            }
            return (ret[0], ret[1]);
        }
    }
}
