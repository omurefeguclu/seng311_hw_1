using Library.Domain;
using Xunit;

namespace Library.Tests
{
    public class PatchScoreTableBasedTests
    {
        // T1 – playerId < 0  → should throw KeyNotFoundException
        [Fact]
        public void PatchScore_PlayerIdNegative_ThrowsKeyNotFound()
        {
            var lb = new Leaderboard { Name = "T1" };
            Assert.Throws<KeyNotFoundException>(() => lb.PatchScore(-5, 10));
        }

        // T2 – newScore < 0  → should throw ArgumentException
        [Fact]
        public void PatchScore_ScoreNegative_ThrowsArgumentException()
        {
            var lb = new Leaderboard { Name = "T2" };
            Assert.Throws<ArgumentException>(() => lb.PatchScore(1, -10));
        }

        // T3 – Name is empty → should throw InvalidOperationException
        [Fact]
        public void PatchScore_NameMissing_ThrowsInvalidOperation()
        {
            var lb = new Leaderboard(); // Name is null
            Assert.Throws<InvalidOperationException>(() => lb.PatchScore(1, 10));
        }

        // T4 – Player not found → should add new record
        [Fact]
        public void PatchScore_NewPlayer_AddsRecord()
        {
            var lb = new Leaderboard { Name = "T4" };
            lb.PatchScore(10, 100);

            Assert.Single(lb.Scores);
            Assert.Equal(100, lb.GetPlayerScore(10));
        }

        // T5 – Player exists → should update record
        [Fact]
        public void PatchScore_ExistingPlayer_UpdatesRecord()
        {
            var lb = new Leaderboard { Name = "T5" };
            lb.PatchScore(1, 50);
            lb.PatchScore(1, 75);

            Assert.Single(lb.Scores);
            Assert.Equal(75, lb.GetPlayerScore(1));
        }
    }
}