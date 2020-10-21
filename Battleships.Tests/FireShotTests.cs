using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Tests
{
    class FireShotTests
    {
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
            for (int x = 1; x <= gameOptions.BoardWidth; x++)
            {
                for (int y = 1; y <= gameOptions.BoardHeight; y++)
                {
                    var shotResult = game.Fire((x, y));
                    if (shotResult == ShotResult.Hit || shotResult == ShotResult.Sunk || shotResult == ShotResult.GameWon)
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
            for (int x = 1; x <= gameOptions.BoardWidth; x++)
            {
                for (int y = 1; y <= gameOptions.BoardHeight; y++)
                {
                    var shotResult = game.Fire((x, y));
                    if (shotResult == ShotResult.Sunk || shotResult == ShotResult.GameWon)
                    {
                        sunkCount++;
                    }
                }
            }

            Assert.AreEqual(sunkCount, gameOptions.Ships.Count());
        }
    }
}
