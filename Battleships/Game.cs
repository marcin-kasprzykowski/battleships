using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships
{
    public class Game
    {
        private const int ShipArrangeRetryLimit = 10;

        public Board Board { get; private set; }
        private List<Ship> ships;
        public GameOptions GameOptions { get; private set; }

        public ShotResult Fire((int x, int y) coordinates)
        {
            (int x, int y) zeroBasedCoordinates = (coordinates.x - 1, coordinates.y - 1);

            if (Board == null)
            {
                throw new InvalidOperationException("Board must be initialized before any shots are fired.");
            }

            if (zeroBasedCoordinates.x >= GameOptions.BoardWidth || zeroBasedCoordinates.y >= GameOptions.BoardHeight || zeroBasedCoordinates.x < 0 || zeroBasedCoordinates.y < 0)
            {
                throw new InvalidOperationException("The shot was fired outside of the board.");
            }

            var boardFieldStatus = Board.GetFieldStatus(zeroBasedCoordinates);
            Board.AddFlag(zeroBasedCoordinates, BoardFieldStatus.AlreadyFiredUpon);

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
            Board = new Board();
            Board.SetupBoard(gameOptions);
            ArrangeShips(gameOptions);
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
                            if (Board.GetFieldStatus((isHorizontal ? placementStartIndex + i : columnOrRowIndex, isHorizontal ? columnOrRowIndex : placementStartIndex + i)) == BoardFieldStatus.Ship)
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
                                Board.SetFlag(coordinates, BoardFieldStatus.Ship);
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
            //there must be exactly one hit ship put in these coordinates
            var damagedShip = ships.Single(x => x.Coordinates.Any(y => y == coordinates));

            //check if all ship's chunks have already been hit
            foreach (var damagedShipCoordinate in damagedShip.Coordinates)
            {
                //the current shot coordinates are not to be checked
                if (damagedShipCoordinate != coordinates)
                {
                    //if at least one chunk has not been hit yet
                    if (!Board.GetFieldStatus(damagedShipCoordinate).HasFlag(BoardFieldStatus.AlreadyFiredUpon))
                    {
                        return null;
                    }
                }
            }
            return damagedShip;
        }
    }
}