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
        private int n_visual_rows;

        public World(int length, int height)
        {
            this.length = length;
            this.height = height;

            this.n_visual_rows = height + 2;

            InitGrid();
            LinkNeighbors();
        }

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
        
        public void Toggle(int i, int j){ this.grid[i, j].Toggle(); }

        public void Cycle()
        {
            bool[,] future_grid = new bool[length, height];

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
                    grid[i, j].SetStatus(future_grid[i, j]);
                }
            }
        }

        private string[] GetVisualRows()
        {
            string[] arr = new string[n_visual_rows];
            string frame = new string('-', length);
            frame = ' ' + frame + ' ';
            arr[0] = frame;
            arr[n_visual_rows - 1] = frame;

            for (int i = 1; i < n_visual_rows - 1; i++)
            {
                string row = "";
                for (int j = 0; j < length; j++)
                {
                    row += this.grid[i-1, j].Show();
                }
                row = '|' + row + '|';
                arr[i] = row;
            }

            return arr;
        }

        private string GetVisualGrid()
        {
            string ret = "";
            string[] rows = GetVisualRows();
            foreach (string row in rows)
            {
                ret += row + '\n';
            }
            return ret;
        }

        public void Show(bool overwrite = true)
        {
            Console.Write(GetVisualGrid());
            if (overwrite) { Console.SetCursorPosition(0, Console.CursorTop - n_visual_rows); }
        }
    }
}
