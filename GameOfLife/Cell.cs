using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Cell
    {
        private bool is_alive;
        private Cell[] neighbors;
        private int n_neighbors;

        public Cell() {
            is_alive = false;
            neighbors = new Cell[8];
            n_neighbors = 0;
        }

        public bool IsAlive() {  return is_alive; }
        public void Toggle() { is_alive = !is_alive; }

        public void SetStatus(bool status) {  is_alive = status; }
        public void AddNeighbor(Cell cell) {
            neighbors[n_neighbors] = cell;
            n_neighbors++;
        }
        public bool GetFutureStatus()
        {
            int n_live_neighbors = GetNLiveNeighbors();
            if (is_alive)
            {
                if (n_live_neighbors < 2 || n_live_neighbors > 3) { return false; }
                return true;
            }
            return n_live_neighbors == 3;

        }

        private int GetNLiveNeighbors()
        {
            int n_live = 0;
            for(int i = 0; i < n_neighbors; i++) { 
                if (neighbors[i].IsAlive()) { n_live++; }
            }

            return n_live;
        }
        public string Show()
        {
            string ret = is_alive ? "o" : " ";
            return ret;
        }
    }
}
