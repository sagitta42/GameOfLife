using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class World
    {
        public int length;
        public int height;

        private Cell[,] grid;

        public bool is_populated;
        public bool is_stable;

        public World(int length, int height)
        {
            this.length = length;
            this.height = height;

            InitGrid();
            LinkNeighbors();
        }

        public World((int, int) size): this(size.Item1, size.Item2) { }

        private void InitGrid()
        {
            this.grid = new Cell[length, height];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    this.grid[i, j] = new Cell();
                }
            }
        }

        private void LinkNeighbors()
        {
            // TODO: #9 improve algorithm
            int counter_progress = 0;
            int n_total = length * height;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i_shift = -1; i_shift < 2; i_shift++)
                    {
                        for (int j_shift = -1; j_shift < 2; j_shift++)
                        {
                            int i_neighbor = i + i_shift;
                            int j_neighbor = j + j_shift;
                            if (!IsInGrid(i_neighbor, j_neighbor) || (i_neighbor == i && j_neighbor == j)) { continue; }

                            this.grid[i, j].AddNeighbor(grid[i_neighbor, j_neighbor]);
                        }
                    }
                    counter_progress++;
                }
            }
        }

        private bool IsInGrid(int i, int j)
        {
            bool ret = i >= 0 && i < length && j >= 0 && j < height;
            return ret;
        }
        
        public bool IsInGrid((int, int) coord)
        {
            return IsInGrid(coord.Item1, coord.Item2);
        }

        public Cell GetCell(int i, int j)
        {
            return IsInGrid(i, j) ? grid[i, j] : null;
        }

        public void ToggleCell(int i, int j){ this.grid[i, j].Toggle(); }

        public void ToggleCell((int, int) coord) { this.ToggleCell(coord.Item1, coord.Item2); }
        public void Cycle()
        {
            bool[,] future_grid = new bool[length, height];
            is_populated = false;
            is_stable = true;
            // TODO: #4 optimize by scanning only relevant cells instead of all
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    future_grid[i, j] = grid[i, j].GetFutureStatus();
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    is_populated = is_populated || future_grid[i, j];
                    is_stable = is_stable && future_grid[i, j] == grid[i, j].IsAlive();
                    grid[i, j].SetStatus(future_grid[i, j]);
                }
            }
        }
    }
}
