using System;

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