using System;
using System.Linq;

namespace Battleships
{
    internal class Program
    {
        private static Game gameObject = new Game();

        private static void Main(string[] args)
        {
            gameObject.Initialize(new GameOptions { BoardHeight = 3, BoardWidth = 3, Ships = new System.Collections.Generic.List<int> { 3 } });

            Console.WriteLine("Welcome to BATTLESHIPS!");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"New game has been started. The board size is {gameObject.GameOptions.BoardWidth}x{gameObject.GameOptions.BoardHeight}.");
            Console.WriteLine($"The following ships have been placed: {String.Join(", ", gameObject.GameOptions.Ships.Select(x => x.ToString()))}.");
            Console.WriteLine("-----------------------");
            Console.WriteLine();

            while (true)
            {
                DisplayBoard();
                Console.WriteLine("Enter coordinates of your next shot (eg. '5,8'):");
                var coordinates = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var parsedCoordinates = ParseCoordinates(coordinates);
                if (parsedCoordinates == null)
                {
                    Console.WriteLine("You have entered invalid coordinates.");
                    continue;
                }

                var shotResult = gameObject.Fire(parsedCoordinates.Value);
                switch (shotResult)
                {
                    case ShotResult.Miss:
                        Console.WriteLine("You missed!");
                        break;
                    case ShotResult.Hit:
                        Console.WriteLine("You hit a ship!");
                        break;
                    case ShotResult.Sunk:
                        Console.WriteLine("You sunk the ship!");
                        break;
                    case ShotResult.GameWon:
                        Console.WriteLine("You WON the game, congratulations!");
                        Console.WriteLine("The game is now finished. Press any key to close the window.");
                        Console.Read();
                        return;
                }
            }
        }

        private static (int x, int y)? ParseCoordinates(string[] coordinates)
        {
            if (coordinates.Length != 2)
            {
                return null;
            }

            if (int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
            {
                if (x <= gameObject.Board.GetUpperBound(0)+1 && y <= gameObject.Board.GetUpperBound(1)+1)
                {
                    return (x, y);
                }
            }

            return null;
        }

        private static void DisplayBoard()
        {
            Console.WriteLine("=======================");
            Console.WriteLine("Current board:");
            Console.Write("  ");
            for (int i = 0; i <= gameObject.Board.GetUpperBound(0); i++)
            {
                Console.Write(i + 1);
            }
            Console.WriteLine();
            for (int x = 0; x <= gameObject.Board.GetUpperBound(0); x++)
            {
                Console.Write((x + 1).ToString().PadRight(2));
                for (int y = 0; y <= gameObject.Board.GetUpperBound(1); y++)
                {
                    Console.Write(GetFieldStatusAbbreviation(gameObject.Board[x, y]));
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Legend: unknown: {GetFieldStatusAbbreviation(BoardFieldStatus.Empty)}  hit: {GetFieldStatusAbbreviation(BoardFieldStatus.Ship | BoardFieldStatus.AlreadyFiredUpon)}  miss: {GetFieldStatusAbbreviation(BoardFieldStatus.Empty | BoardFieldStatus.AlreadyFiredUpon)}");
            Console.WriteLine("=======================");
            Console.WriteLine();
        }

        private static string GetFieldStatusAbbreviation(BoardFieldStatus status)
        {
            if (!status.HasFlag(BoardFieldStatus.AlreadyFiredUpon))
            {
                return ".";
            }
            if (status.HasFlag(BoardFieldStatus.Empty))
            {
                return "o";
            }
            if (status.HasFlag(BoardFieldStatus.Ship))
            {
                return "X";
            }

            throw new ArgumentException("Invalid board field status");
        }
    }
}