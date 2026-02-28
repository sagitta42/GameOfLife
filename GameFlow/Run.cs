using GameLogic;

namespace GameFlow
{
    public class Run
    {
        public static void RunGame(World world, IGameInterface gameInterface, int cycle_time)
        {
            gameInterface.Show(world, sleep_time: cycle_time);
            bool run_flag = true;
            while (run_flag)
            {
                world.Cycle();
                gameInterface.Show(world, sleep_time: cycle_time, overwrite: true);

                // TODO: #17 detect oscillator and stop program
                if (world.is_stable)
                {
                    gameInterface.Info("STABLE", sleep_time: cycle_time);
                    run_flag = false;
                }
                if (!world.is_populated)
                {
                    gameInterface.Info("THE END", sleep_time: cycle_time);
                    run_flag = false;
                }

            }
        }
    }
}
