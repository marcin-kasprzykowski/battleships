using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships
{
    public class Ship
    {
        public bool IsHorizontal { get; set; }
        public List<(int x, int y)> Coordinates { get; set; }
        public bool IsSunk { get; set; }
    }
}
