using GameLogic;

namespace GameAdapter
{
    public static class TextAdapter
    {
        public static string GetWorldString(World world, string break_char = "\n")
        {
            string[] rows = GetWorldRows(world);
            string ret = "";
            foreach (string row in rows)
            {
                ret = ret + row + break_char;
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
                    row += CellRepr(world.GetCell(i, j - 1));
                }
                row = '|' + row + '|';
                arr[j] = row;
            }

            return arr;
        }
        private static string CellRepr(Cell cell)
        {
            string ret = cell.IsAlive() ? "o" : " ";
            return ret;
        }
    }
}
