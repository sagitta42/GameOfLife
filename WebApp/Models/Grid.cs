namespace GameOfLife.Models
{
    public class Grid
    {
        // TODO: #26 init World with this Grid/grid directly
        public bool[][] grid {get; set;}

        public int GetLength() { return grid[0].Length;}
        public int GetHeight() { return grid.Length; }

        public bool IsAlive(int i, int j) { return grid[j][i]; }
    }
}
