using Xunit;
using Library.Domain;
using Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Tests
{
    public class UserProfileHelperTests
    {
        [Fact]
        public void UserProfileModel_CanBeCreatedWithProperties()
        {
            var model = new UserProfileModel
            {
                PlayerId = 1,
                PlayerName = "TestPlayer",
                LastGames = new List<Game>(),
                LeaderboardScores = new List<LeaderboardScoreOverview>()
            };
            Assert.Equal(1, model.PlayerId);
            Assert.Equal("TestPlayer", model.PlayerName);
            Assert.Empty(model.LastGames);
            Assert.Empty(model.LeaderboardScores);
        }

        [Fact]
        public void GetUserProfile_ReturnsCorrectPlayerIdAndName()
        {
            var player = new Player { Id = 1, Name = "TestPlayer" };
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            gameStore.AddGame(new Game { Player = player, PlayedAt = DateTime.Now, Score = 10 });
            var profile = UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager);
            Assert.Equal(1, profile.PlayerId);
            Assert.Equal("TestPlayer", profile.PlayerName);
        }

        [Fact]
        public void GetUserProfile_ReturnsLastGamesSortedAndLimited()
        {
            var player = new Player { Id = 1, Name = "TestPlayer" };
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            for (int i = 0; i < 15; i++)
            {
                gameStore.AddGame(new Game { Player = player, PlayedAt = DateTime.Now.AddDays(-i), Score = i });
            }
            var profile = UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager, lastGamesCount: 10);
            Assert.Equal(10, profile.LastGames.Count);
            Assert.True(profile.LastGames.SequenceEqual(profile.LastGames.OrderByDescending(g => g.PlayedAt)));
        }

        [Fact]
        public void GetUserProfile_ReturnsLeaderboardScores()
        {
            var player = new Player { Id = 1, Name = "TestPlayer" };
            var gameStore = new GameStore();
            var leaderboardManager = new LeaderboardManager();
            gameStore.AddGame(new Game { Player = player, PlayedAt = DateTime.Now, Score = 10 });
            var lb = leaderboardManager.GetOrAddLeaderboard("TestLB");
            lb.PatchScore(1, 99);
            var profile = UserProfileHelper.GetUserProfile(1, gameStore, leaderboardManager);
            Assert.Single(profile.LeaderboardScores);
            Assert.Equal("TestLB", profile.LeaderboardScores[0].LeaderboardName);
            Assert.Equal(99, profile.LeaderboardScores[0].PlayerScore);
        }

    }
}

