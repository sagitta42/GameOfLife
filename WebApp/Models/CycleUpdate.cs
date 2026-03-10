namespace GameOfLife.Models
{
    public class CycleUpdate
    {
        // FIXME: #26 duplication
        public bool[][] grid { get; set; }

        public string game_status { get; set; }
        public string message { get; set; }
    }
}
