using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Battleships.Tests
{
    class GameTests
    {
        [Test]
        public void StartSimpleGame()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 1 }
            };

            game.Initialize(gameOptions);
        }

        [Test]
        public void StartGameWithNoBoard()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 0,
                BoardWidth = 0,
                Ships = new List<int> { 1 }
            };

            Assert.Throws(typeof(ArgumentException), () => game.Initialize(gameOptions));
        }

        [Test]
        public void StartGameWithTooManyShips()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 2,
                BoardWidth = 2,
                Ships = new List<int> { 8, 4, 6, 4, 4, 7, 5, 7, 9, 5, 8, 9, 6, 2 }
            };

            Assert.Throws(typeof(InvalidOperationException), () => game.Initialize(gameOptions));
        }

        [Test]
        public void StartGameWithTooBigShips()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 11 }
            };

            Assert.Throws(typeof(InvalidOperationException), () => game.Initialize(gameOptions));
        }

        [Test]
        public void StartGameWithBigBoard()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10000,
                BoardWidth = 10000,
                Ships = new List<int> { 8, 4, 6, 4, 4, 7, 5, 7, 9, 5, 8, 9, 6, 2 }
            };

            game.Initialize(gameOptions);
        }

        [Test]
        public void StartGameWithEmptyOptions()
        {
            var game = new Game();

            var gameOptions = new GameOptions();

            Assert.Throws(typeof(ArgumentException), () => game.Initialize(gameOptions));
        }

        [Test]
        public void StartGameWithNullOptions()
        {
            var game = new Game();

            Assert.Throws(typeof(ArgumentException), () => game.Initialize(null));
        }

        [Test]
        public void FireSimpleShot()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 1, 2, 3 }
            };

            game.Initialize(gameOptions);
            game.Fire((5, 5));
        }

        [Test]
        public void FireShotOutsideOfTheBoard()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 1, 2, 3 }
            };

            game.Initialize(gameOptions);
            Assert.Throws(typeof(InvalidOperationException), () => game.Fire((5, 11)));
            Assert.Throws(typeof(InvalidOperationException), () => game.Fire((11, 5)));
        }

        [Test]
        public void FireShotTwice()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 1, 2, 3 }
            };

            game.Initialize(gameOptions);
            Assert.AreEqual(game.Fire((4, 4)), game.Fire((4, 4)));
        }

        [Test]
        public void FireSimpleShotWhenBoardIsNotInitialized()
        {
            var game = new Game();

            Assert.Throws(typeof(InvalidOperationException), () => game.Fire((5, 5)));
        }
        
        [Test]
        public void FireShotsAndAssertHitCount()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 5,
                BoardWidth = 5,
                Ships = new List<int> { 1, 2, 3 }
            };

            game.Initialize(gameOptions);

            int hitCount = 0;
            for (int x = 0; x < gameOptions.BoardWidth; x++)
            {
                for (int y = 0; y < gameOptions.BoardHeight; y++)
                {
                    var shotResult = game.Fire((x, y));
                    if (shotResult == ShotResult.Hit  || shotResult == ShotResult.Sunk)
                    {
                        hitCount++;
                    }
                }
            }

            Assert.AreEqual(hitCount, gameOptions.Ships.Sum());
        }

        [Test]
        public void FireShotsAndAssertSunkCount()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 5,
                BoardWidth = 5,
                Ships = new List<int> { 1, 2, 3 }
            };

            game.Initialize(gameOptions);

            int sunkCount = 0;
            for (int x = 0; x < gameOptions.BoardWidth; x++)
            {
                for (int y = 0; y < gameOptions.BoardHeight; y++)
                {
                    if (game.Fire((x, y)) == ShotResult.Sunk)
                    {
                        sunkCount++;
                    }
                }
            }

            Assert.AreEqual(sunkCount, gameOptions.Ships.Count());
        }
    }
}
