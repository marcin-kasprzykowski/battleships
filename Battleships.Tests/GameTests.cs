using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 8, 4, 6, 4, 4, 7, 5, 7, 9, 5, 8, 9, 6, 2 }
            };

            game.Initialize(gameOptions);
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

            game.Initialize(gameOptions);
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
            game.Fire(5, 5);
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
            Assert.Throws(typeof(InvalidOperationException), () => game.Fire(50, 5));
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
            Assert.AreEqual(game.Fire(4, 4), game.Fire(4, 4));
        }

        [Test]
        public void FireSimpleShotWhenBoardIsNotInitialized()
        {
            var game = new Game();

            Assert.Throws(typeof(InvalidOperationException), () => game.Fire(5, 5));
        }
    }
}
