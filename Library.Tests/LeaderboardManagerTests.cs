using Xunit;
using Library.Domain;
using System.Collections.Generic;

namespace Library.Tests
{
    public class LeaderboardManagerTests
    {
        [Fact]
        public void AddLeaderboard_AddsLeaderboard()
        {
            var manager = new LeaderboardManager();
            manager.AddLeaderboard("Test");
            var lb = manager.GetLeaderboard("Test");
            Assert.NotNull(lb);
            Assert.Equal("Test", lb.Name);
        }

        [Fact]
        public void AddLeaderboard_DoesNotAddDuplicate()
        {
            var manager = new LeaderboardManager();
            manager.AddLeaderboard("Test");
            manager.AddLeaderboard("Test");
            Assert.Single(manager.GetLeaderboardNames());
        }

        [Fact]
        public void GetLeaderboard_ReturnsNullIfNotExists()
        {
            var manager = new LeaderboardManager();
            Assert.Null(manager.GetLeaderboard("Missing"));
        }

        [Fact]
        public void GetOrAddLeaderboard_ReturnsExisting()
        {
            var manager = new LeaderboardManager();
            manager.AddLeaderboard("Test");
            var lb1 = manager.GetOrAddLeaderboard("Test");
            var lb2 = manager.GetLeaderboard("Test");
            Assert.Same(lb1, lb2);
        }

        [Fact]
        public void GetOrAddLeaderboard_CreatesIfMissing()
        {
            var manager = new LeaderboardManager();
            var lb = manager.GetOrAddLeaderboard("New");
            Assert.NotNull(lb);
            Assert.Equal("New", lb.Name);
        }

        [Fact]
        public void GetLeaderboardNames_ReturnsAllNames()
        {
            var manager = new LeaderboardManager();
            manager.AddLeaderboard("A");
            manager.AddLeaderboard("B");
            var names = manager.GetLeaderboardNames();
            Assert.Contains("A", names);
            Assert.Contains("B", names);
            Assert.Equal(2, new List<string>(names).Count);
        }

        [Fact]
        public void GetAllScoresForPlayer_ReturnsScoresAcrossLeaderboards()
        {
            var manager = new LeaderboardManager();
            var lb1 = manager.GetOrAddLeaderboard("LB1");
            var lb2 = manager.GetOrAddLeaderboard("LB2");
            lb1.PatchScore(1, 10);
            lb2.PatchScore(1, 20);
            var scores = manager.GetAllScoresForPlayer(1);
            Assert.Contains(scores, s => s.LeaderboardName == "LB1" && s.PlayerScore == 10);
            Assert.Contains(scores, s => s.LeaderboardName == "LB2" && s.PlayerScore == 20);
            Assert.Equal(2, scores.Count);
        }

        [Fact]
        public void GetAllScoresForPlayer_ReturnsZeroIfPlayerNotFound()
        {
            var manager = new LeaderboardManager();
            manager.AddLeaderboard("LB1");
            manager.AddLeaderboard("LB2");
            var scores = manager.GetAllScoresForPlayer(99);
            Assert.All(scores, s => Assert.Equal(0, s.PlayerScore));
        }

        [Fact]
        public void EmptyManager_ReturnsEmptyNamesAndScores()
        {
            var manager = new LeaderboardManager();
            Assert.Empty(manager.GetLeaderboardNames());
            Assert.Empty(manager.GetAllScoresForPlayer(1));
        }
    }
}

