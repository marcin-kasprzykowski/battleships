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

            game.Initialize(gameOptions);
        }

        [Test]
        public void StartGameWithTooManyShips()
        {
            var game = new Game();

            var gameOptions = new GameOptions()
            {
                BoardHeight = 10,
                BoardWidth = 10,
                Ships = new List<int> { 8,4,6,4,4,7,5,7,9,5,8,9,6,2 }
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
        public void StartGameWithNoOptions()
        {
            var game = new Game();

            var gameOptions = new GameOptions();

            game.Initialize(gameOptions);
        }
    }
}
