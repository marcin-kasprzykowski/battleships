using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships
{
    public class Game
    {
        private const int ShipArrangeRetryLimit = 10;

        BoardFieldStatus[,] board;
        public BoardFieldStatus[,] Board { get { return board.Clone() as BoardFieldStatus[,]; } }
        private GameStatus gameStatus = GameStatus.New;
        private List<Ship> ships;
        public GameOptions GameOptions { get; private set; }

        public ShotResult Fire((int x, int y) coordinates)
        {
            (int x, int y) zeroBasedCoordinates = (coordinates.x - 1, coordinates.y - 1);

            if (gameStatus != GameStatus.BoardInitialized)
            {
                throw new InvalidOperationException("Board must be initialized before any shots are fired.");
            }

            if (zeroBasedCoordinates.x > board.GetUpperBound(0) || zeroBasedCoordinates.y > board.GetUpperBound(1) || zeroBasedCoordinates.x < 0 || zeroBasedCoordinates.y < 0)
            {
                throw new InvalidOperationException("The shot was fired outside of the board.");
            }

            var boardFieldStatus = board[zeroBasedCoordinates.x, zeroBasedCoordinates.y];
            board[zeroBasedCoordinates.x, zeroBasedCoordinates.y] |= BoardFieldStatus.AlreadyFiredUpon;

            if (boardFieldStatus.HasFlag(BoardFieldStatus.Ship))
            {
                var damagedShip = IsLastChunkOfShip(zeroBasedCoordinates);

                if (damagedShip != null)
                {
                    damagedShip.IsSunk = true;
                    if (ships.All(x => x.IsSunk))
                    {
                        return ShotResult.GameWon;
                    }
                    return ShotResult.Sunk;
                }
                return ShotResult.Hit;
            }

            return ShotResult.Miss;
        }

        public void Initialize(GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            if (gameOptions == null)
            {
                throw new ArgumentException("Game options cannot be null.");
            }

            if (gameOptions.BoardHeight < 1 || gameOptions.BoardWidth < 1)
            {
                throw new ArgumentException("Board dimensions must have a positive values.");
            }

            SetupBoard(gameOptions);
        }

        private void ArrangeShips(GameOptions gameOptions)
        {
            ships = new List<Ship>();
            Random random = new Random();

            foreach (var ship in gameOptions.Ships)
            {
                for (int retryCount = 0; retryCount < ShipArrangeRetryLimit; retryCount++)
                {
                    bool shipProperlyPlaced = true;

                    //roll ship's orientation
                    var isHorizontal = random.Next(0, 2) == 0;

                    var columnOrRowIndex = random.Next(0, (isHorizontal ? gameOptions.BoardHeight : gameOptions.BoardWidth) - 1);
                    //when placing the ship its most left-hand part is taken into consideration. Therefore its starting position must not be too close to the board's right border.
                    int maxPossiblePosition = (isHorizontal ? gameOptions.BoardWidth : gameOptions.BoardHeight) - 1 - ship + 1;
                    if (maxPossiblePosition < 0)
                    {
                        shipProperlyPlaced = false;
                    }
                    else
                    {
                        var placementStartIndex = random.Next(0, maxPossiblePosition);

                        //check whether proposed placement is correct (there must not be any other ships on any of the considered fields)
                        for (int i = 0; i < ship; i++)
                        {
                            if (board[isHorizontal ? placementStartIndex + i : columnOrRowIndex, isHorizontal ? columnOrRowIndex : placementStartIndex + i] == BoardFieldStatus.Ship)
                            {
                                //another ship is already present on this field. Placement retry will occur
                                shipProperlyPlaced = false;
                            }
                        }

                        if (shipProperlyPlaced)
                        {
                            //place the ship on the board
                            List<(int x, int y)> coordinatesList = new List<(int x, int y)>();
                            for (int i = 0; i < ship; i++)
                            {
                                (int x, int y) coordinates = (isHorizontal ? placementStartIndex + i : columnOrRowIndex, isHorizontal ? columnOrRowIndex : placementStartIndex + i);
                                board[coordinates.x, coordinates.y] = BoardFieldStatus.Ship;
                                coordinatesList.Add(coordinates);
                            }

                            ships.Add(new Ship { IsHorizontal = isHorizontal, Coordinates = coordinatesList });
                        }
                    }
                    if (shipProperlyPlaced)
                    {
                        break;
                    }

                    if (!shipProperlyPlaced && retryCount == ShipArrangeRetryLimit - 1)
                    {
                        throw new InvalidOperationException("Ship placement failed. Perhaps there were too many ships?");
                    }
                }
            }
        }

        private Ship IsLastChunkOfShip((int x, int y) coordinates)
        {
            //there must be exactly one hit ship
            var damagedShip = ships.Single(x => x.Coordinates.Any(y => y == coordinates));

            //check if all ship's chunks have already been hit
            foreach (var damagedShipCoordinate in damagedShip.Coordinates)
            {
                //the current shot coordinates are not to be checked
                if (damagedShipCoordinate != coordinates)
                {
                    //if at least one chunk has not been hit yet
                    if (!board[damagedShipCoordinate.x, damagedShipCoordinate.y].HasFlag(BoardFieldStatus.AlreadyFiredUpon))
                    {
                        return null;
                    }
                }
            }
            return damagedShip;
        }

        private void SetupBoard(GameOptions gameOptions)
        {
            board = new BoardFieldStatus[gameOptions.BoardWidth, gameOptions.BoardHeight];
            for (int x = 0; x <= board.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= board.GetUpperBound(1); y++)
                {
                    board[x, y] = BoardFieldStatus.Empty;
                }
            }
            ArrangeShips(gameOptions);

            gameStatus = GameStatus.BoardInitialized;
        }
    }
}