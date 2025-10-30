using Library.Domain;

namespace Library.Tests
{
    public class PatchScoreWhiteBoxTests
    {
        [Fact]
        public void PatchScore_Throws_ForNegativePlayerId()
        {
            var lb = new Leaderboard { Name = "WB" };
            Assert.Throws<KeyNotFoundException>(() => lb.PatchScore(-1, 10));
        }

        [Fact]
        public void PatchScore_Throws_ForNegativeScore()
        {
            var lb = new Leaderboard { Name = "WB" };
            Assert.Throws<ArgumentException>(() => lb.PatchScore(1, -5));
        }

        [Fact]
        public void PatchScore_Throws_WhenNameNotSet()
        {
            var lb = new Leaderboard(); // Name is null
            Assert.Throws<InvalidOperationException>(() => lb.PatchScore(1, 10));
        }

        [Fact]
        public void PatchScore_AddsNewRecord_WhenPlayerNotPresent()
        {
            var lb = new Leaderboard { Name = "AddTest" };
            Assert.Empty(lb.Scores);

            lb.PatchScore(42, 9001);

            Assert.Single(lb.Scores);
            var sc = lb.Scores.First();
            Assert.Equal(42, sc.Player.Id);
            // Player.Name wasn't provided when adding; implementation creates new Player { Id = ... }
            Assert.Null(sc.Player.Name);
            Assert.Equal(9001, sc.Score);
            Assert.Equal(9001, lb.GetPlayerScore(42));
        }

        [Fact]
        public void PatchScore_UpdatesExistingRecord_AndPreservesPlayerName()
        {
            var lb = new Leaderboard { Name = "UpdateTest" };
            lb.PatchScore(7, 100);
            // set a name to ensure it's preserved
            lb.ChangePlayerName(7, "OriginalName");
            var beforeCount = lb.Scores.Count;

            lb.PatchScore(7, 777);

            // count should not have increased
            Assert.Equal(beforeCount, lb.Scores.Count);
            Assert.Equal(777, lb.GetPlayerScore(7));
            var sc = lb.Scores.First(s => s.Player.Id == 7);
            Assert.Equal("OriginalName", sc.Player.Name);
        }
    }
}