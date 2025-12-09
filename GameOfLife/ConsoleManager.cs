using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class ConsoleManager
    {
        private int n_world_rows;
        private int n_lines_skip = 0;

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message = "")
        {
            Console.WriteLine(message);
            n_lines_skip++;
        }

        public string ReadLine()
        {
            string user_input = Console.ReadLine();
            n_lines_skip++;
            return user_input;
        }

        public void Show(World world, int cycle_time = 0, bool overwrite = false)
        {
            if (overwrite) {
                Console.SetCursorPosition(0, Console.CursorTop - n_world_rows - n_lines_skip);
            }
            string[] rows = GetWorldRows(world);
            foreach(string row in rows)
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
            System.Threading.Thread.Sleep(cycle_time);
        }

        private string[] GetWorldRows(World world)
        {
            this.n_world_rows = world.height + 2;

            string[] arr = new string[n_world_rows];
            string frame = new string('-', world.length);
            frame = ' ' + frame + ' ';
            arr[0] = frame;
            arr[n_world_rows - 1] = frame;

            for (int j = 1; j < n_world_rows - 1; j++)
            {
                string row = "";
                for (int i = 0; i < world.length; i++)
                {
                    row += CellRepr(world.GetCell(i, j-1));
                }
                row = '|' + row + '|';
                arr[j] = row;
            }

            return arr;
        }
        private string CellRepr(Cell cell)
        {
            string ret = cell.IsAlive() ? "o" : " ";
            return ret;
        }
    }
}
