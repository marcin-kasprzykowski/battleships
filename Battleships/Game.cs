using System;
using System.Collections.Generic;
using System.Linq;
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

            Console.WriteLine("Board has been successfully created with the following options:");
            Console.WriteLine($"Board size:{ gameOptions.BoardWidth} x { gameOptions.BoardHeight}");
            Console.WriteLine($"Ships collection: {gameOptions.Ships.OrderByDescending(x => x)}");
        }

        public ShotStatus Fire(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
