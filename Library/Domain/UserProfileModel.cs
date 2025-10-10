using System.Collections.Generic;

namespace Library.Domain
{
    public class UserProfileModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public List<Game> LastGames { get; set; } = new();
        public List<LeaderboardScoreOverview> LeaderboardScores { get; set; } = new();
    }
}