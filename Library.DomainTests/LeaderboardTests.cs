using Xunit;
using Library.Domain;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    public class LeaderboardTests
    {
        [Theory]
        [InlineData(1, "Alice", 100)]
        [InlineData(2, "Bob", 200)]
        [InlineData(3, "Charlie", 0)]
        public void GetPlayerScore_ShouldReturnExpectedScore(int playerId, string playerName, int expectedScore)
        {
            // Arrange
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(1, 100);
            leaderboard.PatchScore(2, 200);
            leaderboard.ChangePlayerName(1, "Alice");
            leaderboard.ChangePlayerName(2, "Bob");

            // Act
            int actualScore = leaderboard.GetPlayerScore(playerId);

            // Assert
            Assert.Equal(expectedScore, actualScore);
            // TODO: Add more parameterized tests for other methods
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(2, 100)]
        public void GetPlayerScore_ReturnsPlayerScore(int playerId, int score)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(playerId, score);
            Assert.Equal(score, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(99)]
        [InlineData(100)]
        public void GetPlayerScore_ReturnsZero_WhenPlayerDoesNotExist(int playerId)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            Assert.Equal(0, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(1, 10, 20)]
        [InlineData(2, 30, 40)]
        public void PatchScore_UpdatesExistingPlayer(int playerId, int initialScore, int updatedScore)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(playerId, initialScore);
            leaderboard.PatchScore(playerId, updatedScore);
            Assert.Equal(updatedScore, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(2, 30)]
        [InlineData(3, 40)]
        public void PatchScore_AddsNonExistingPlayer(int playerId, int score)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(playerId, score);
            Assert.Equal(score, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(1, 10, 5, 15)]
        [InlineData(2, 20, 10, 30)]
        public void IncreaseScore_UpdatesExistingRecord(int playerId, int initialScore, int increment, int expectedScore)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(playerId, initialScore);
            leaderboard.IncreaseScore(playerId, increment);
            Assert.Equal(expectedScore, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(3, 7)]
        [InlineData(4, 12)]
        public void IncreaseScore_GenerateExistingRecord(int playerId, int increment)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.IncreaseScore(playerId, increment);
            Assert.Equal(increment, leaderboard.GetPlayerScore(playerId));
        }

        [Theory]
        [InlineData(1, "NewName")]
        [InlineData(2, "AnotherName")]
        public void ChangePlayerName_ChangesExistingName(int playerId, string newName)
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(playerId, 10);
            leaderboard.ChangePlayerName(playerId, newName);
            var score = leaderboard.Scores.Find(s => s.Player.Id == playerId);
            Assert.Equal(newName, score.Player.Name);
        }

        [Fact]
        public void ChangePlayerName_ThrowsIfPlayerNotFound()
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            Assert.Throws<KeyNotFoundException>(() => leaderboard.ChangePlayerName(99, "Name"));
        }

        [Fact]
        public void Clear_RemovesAllScores()
        {
            var leaderboard = new Leaderboard { Name = "TestBoard" };
            leaderboard.PatchScore(1, 10);
            leaderboard.PatchScore(2, 20);
            leaderboard.Clear();
            Assert.Empty(leaderboard.Scores);
        }
    }
}
