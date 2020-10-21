using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Battleships.Tests
{
    class GameInitializationTests
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
        public void StartComplexGame()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 100,
                BoardWidth = 100,
                Ships = new List<int> { 8, 4, 6, 4, 4, 7, 5, 7, 9, 5, 8, 9, 6, 2 }
            };

            game.Initialize(gameOptions);
        }

        [Test]
        public void StartGameWithShipAsLongAsBoard()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 3,
                BoardWidth = 3,
                Ships = new List<int> { 3 }
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
    }
}
