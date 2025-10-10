using Xunit;
using Library.Domain;
using System;
using System.Linq;

namespace Library.Tests
{
    public class GameStoreTests
    {
        [Fact]
        public void Game_CanBeCreatedWithProperties()
        {
            var player = new Player { Id = 1, Name = "TestPlayer" };
            var playedAt = new DateTime(2025, 10, 11, 15, 0, 0);
            var game = new Game { Player = player, PlayedAt = playedAt, Score = 42 };
            Assert.Equal(player, game.Player);
            Assert.Equal(playedAt, game.PlayedAt);
            Assert.Equal(42, game.Score);
        }

        [Fact]
        public void AddGame_AddsGameToStore()
        {
            var store = new GameStore();
            var game = new Game { Player = new Player { Id = 1 }, PlayedAt = DateTime.Now, Score = 10 };
            store.AddGame(game);
            Assert.Contains(game, store.GetAllGames());
        }

        [Fact]
        public void GetAllGames_ReturnsAllGames()
        {
            var store = new GameStore();
            var game1 = new Game { Player = new Player { Id = 1 }, PlayedAt = DateTime.Now, Score = 10 };
            var game2 = new Game { Player = new Player { Id = 2 }, PlayedAt = DateTime.Now, Score = 20 };
            store.AddGame(game1);
            store.AddGame(game2);
            var allGames = store.GetAllGames();
            Assert.Equal(2, allGames.Count);
            Assert.Contains(game1, allGames);
            Assert.Contains(game2, allGames);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 2, 0)]
        public void GetGamesByPlayer_ReturnsCorrectGames(int playerId, int totalGames, int expectedCount)
        {
            var store = new GameStore();
            store.AddGame(new Game { Player = new Player { Id = 1 }, PlayedAt = DateTime.Now, Score = 10 });
            store.AddGame(new Game { Player = new Player { Id = 2 }, PlayedAt = DateTime.Now, Score = 20 });
            var games = store.GetGamesByPlayer(playerId);
            Assert.Equal(expectedCount, games.Count);
        }

        [Fact]
        public void GetGamesByDate_ReturnsCorrectGames()
        {
            var store = new GameStore();
            var date = new DateTime(2025, 10, 11);
            var game1 = new Game { Player = new Player { Id = 1 }, PlayedAt = date, Score = 10 };
            var game2 = new Game { Player = new Player { Id = 2 }, PlayedAt = date.AddDays(1), Score = 20 };
            store.AddGame(game1);
            store.AddGame(game2);
            var games = store.GetGamesByDate(date);
            Assert.Single(games);
            Assert.Equal(game1, games[0]);
        }

        [Fact]
        public void GetGamesByDate_ReturnsEmptyIfNoMatch()
        {
            var store = new GameStore();
            var date = new DateTime(2025, 10, 11);
            store.AddGame(new Game { Player = new Player { Id = 1 }, PlayedAt = date.AddDays(1), Score = 10 });
            var games = store.GetGamesByDate(date);
            Assert.Empty(games);
        }

        [Fact]
        public void EmptyStore_ReturnsEmptyCollections()
        {
            var store = new GameStore();
            Assert.Empty(store.GetAllGames());
            Assert.Empty(store.GetGamesByPlayer(1));
            Assert.Empty(store.GetGamesByDate(DateTime.Now));
        }
    }
}

