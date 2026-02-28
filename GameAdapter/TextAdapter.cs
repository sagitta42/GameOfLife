using GameLogic;

namespace GameAdapter
{
    public static class TextAdapter
    {
        public static string GetWorldString(World world, string break_char = "\n")
        {
            string[] rows = GetWorldRows(world);
            string ret = "";
            for(int i = 0; i < rows.Length; i ++)
            {
                ret = ret + rows[i];
                if(i < rows.Length - 1) { ret = ret + break_char; }
            }
            return ret;
        }

        public static string[] GetWorldRows(World world)
        {
            int n_world_rows;
            n_world_rows = world.height + 2;

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
                    row += GetCellRepr(world.GetCell(i, j - 1));
                }
                row = '|' + row + '|';
                arr[j] = row;
            }

            return arr;
        }

        public static World GetWorldFromString(string world_repr)
        {
            string[] rows_text = world_repr.Split("\n");

            int world_height = rows_text.Length - 2;
            int world_length = rows_text[0].Length - 2;
            World world = new World(world_length, world_height);

            for (int j = 1; j < rows_text.Length - 1; j++)
            {
                string row = rows_text[j];
                for (int i = 1; i < row.Length - 1; i++)
                {
                    bool cell_alive = GetCellStatus(row[i]);
                    if (cell_alive) { world.ToggleCell(i - 1, j - 1); }
                }
            }
            return world;
        }

        private static bool GetCellStatus(char cell_repr)
        {
            // TODO: move repr to class member
            bool ret = cell_repr == 'o';
            return ret;
        }

        private static string GetCellRepr(Cell cell)
        {
            string ret = cell.IsAlive() ? "o" : " ";
            return ret;
        }
    }
}
