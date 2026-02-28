using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameLogic;

namespace GameFlow
{
    public interface IGameInterface
    {
        void Show(World world, int sleep_time = 0, bool overwrite = false);
        void Info(string message, int sleep_time = 0);
    }
}
