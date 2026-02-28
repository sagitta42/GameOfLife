using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameLogic;
using GameAdapter;

namespace ConsoleApp
{
    public class ConsoleManager
    {
        private int n_lines_skip = 0;

        public void WriteLine(string message = "", int sleep_time = 0)
        {
            Console.WriteLine(message);
            n_lines_skip++;
            System.Threading.Thread.Sleep(sleep_time);
        }

        public string ReadInput(string message)
        {
            Console.Write(message + ": ");
            string user_input = Console.ReadLine();
            n_lines_skip++;
            return user_input;
        }

        public void Show(World world, int sleep_time = 0, bool overwrite = false)
        {
            if (overwrite)
            {
                int n_world_rows;
                n_world_rows = world.height + 2;
                Console.SetCursorPosition(0, Console.CursorTop - n_world_rows - n_lines_skip);
            }
            string[] rows = TextAdapter.GetWorldRows(world);
            foreach (string row in rows)
            {
                Console.WriteLine(row);
            }
            if (overwrite)
            {
                for (int i = 0; i < n_lines_skip; i++) { Console.WriteLine(); }
            }
            else
            {
                n_lines_skip = 0;
            }
            System.Threading.Thread.Sleep(sleep_time);
        }
    }
}
