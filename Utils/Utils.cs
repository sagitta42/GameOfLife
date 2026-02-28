using System.Reflection;

namespace Utils
{
    public static class Utils
    {
        public static string[] GetHeaderLines()
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

            string[] ret = { frame, subframe, header, subframe, frame};
            return ret;
        }

    }
}
