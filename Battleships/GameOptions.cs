using System.Collections.Generic;

namespace Battleships
{
    public class GameOptions
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public List<int> Ships { get; set; }
    }
}