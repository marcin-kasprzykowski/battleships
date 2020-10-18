using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships
{
    [Flags]
    public enum BoardFieldStatus
    {
        Empty = 1,
        Ship = 2,
        AlreadyFiredUpon = 4
    }
}
