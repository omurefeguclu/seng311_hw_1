using System;
using System.Linq;
using Xunit;
using Library.Domain;
using Library.Helpers;
using System.Collections.Generic;

namespace Library.Tests
{
    public class GetUserProfileWhiteBoxTests
    {
        [Fact]
        public void GetUserProfile_ThrowsForNonPositivePlayerId()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            Assert.Throws<ArgumentException>(() => UserProfileHelper.GetUserProfile(0, gameStore, leaderboardManager));
        }

        [Fact]
        public void GetUserProfile_ThrowsForNonPositiveLastGamesCount()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            // playerId validation passes (positive) but lastGamesCount invalid
            Assert.Throws<ArgumentException>(() => UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager, lastGamesCount: 0));
        }

        [Fact]
        public void GetUserProfile_ThrowsWhenNoGames()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            // No games for player -> helper should throw KeyNotFoundException (white-box: playerName null)
            Assert.Throws<KeyNotFoundException>(() => UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager));
        }

        [Fact]
        public void GetUserProfile_ThrowsWhenGamePlayerNameIsEmpty()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            // Add a game but with empty player name -> helper should treat as missing
            gameStore.AddGame(new Game { Player = new Player { Id = 1, Name = string.Empty }, PlayedAt = DateTime.UtcNow, Score = 5 });
            Assert.Throws<KeyNotFoundException>(() => UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager));
        }

        [Fact]
        public void GetUserProfile_ReturnsLeaderboardScores_WhenAvailable()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            var player = new Player { Id = 5, Name = "WhiteBox" };
            gameStore.AddGame(new Game { Player = player, PlayedAt = DateTime.UtcNow, Score = 50 });

            var lb = leaderboardManager.GetOrAddLeaderboard("WB-LB");
            lb.PatchScore(5, 123);

            var profile = UserProfileHelper.GetUserProfile(5, gameStore, leaderboardManager);
            Assert.Equal(5, profile.PlayerId);
            Assert.Equal("WhiteBox", profile.PlayerName);
            Assert.Single(profile.LeaderboardScores);
            Assert.Equal("WB-LB", profile.LeaderboardScores[0].LeaderboardName);
            Assert.Equal(123, profile.LeaderboardScores[0].PlayerScore);
        }

        [Fact]
        public void GetUserProfile_RespectsLastGamesCountAndOrdering()
        {
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            var player = new Player { Id = 7, Name = "Limiter" };

            // Add 5 games with decreasing PlayedAt values
            for (int i = 0; i < 5; i++)
            {
                gameStore.AddGame(new Game { Player = player, PlayedAt = DateTime.UtcNow.AddMinutes(-i), Score = i });
            }

            var profile = UserProfileHelper.GetUserProfile(7, gameStore, leaderboardManager, lastGamesCount: 3);
            Assert.Equal(3, profile.LastGames.Count);
            // Ensure ordering is descending by PlayedAt
            Assert.True(profile.LastGames.SequenceEqual(profile.LastGames.OrderByDescending(g => g.PlayedAt)));
        }
    }
}