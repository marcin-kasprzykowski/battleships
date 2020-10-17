using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships
{
    public class Game
    {
        public void Initialize(GameOptions gameOptions)
        {
            if (gameOptions == null)
            {
                throw new ArgumentException("Game options cannot be null.");
            }

            if (gameOptions.BoardHeight < 1 || gameOptions.BoardWidth < 1)
            {
                throw new ArgumentException("Board dimensions must have a positive values.");
            }
        }
    }
}
