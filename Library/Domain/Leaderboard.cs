using System.Collections.Generic;

namespace Library.Domain
{
    public class Leaderboard
    {
        public string Name { get; set; }
        public List<LeaderboardScore> Scores { get; set; } = new List<LeaderboardScore>();

        public void PatchScore(int playerId, int newScore)
        {
            var score = Scores.Find(s => s.Player.Id == playerId);
            if (score != null)
            {
                score.Score = newScore;
            }
            else
            {
                Scores.Add(new LeaderboardScore { Player = new Player { Id = playerId }, Score = newScore });
            }
        }

        public void IncreaseScore(int playerId, int increment)
        {
            var score = Scores.Find(s => s.Player.Id == playerId);
            if (score != null)
            {
                score.Score += increment;
            }
            else
            {
                Scores.Add(new LeaderboardScore { Player = new Player { Id = playerId }, Score = increment });
            }
        }

        public void Clear()
        {
            Scores.Clear();
        }

        public void ChangePlayerName(int playerId, string newName)
        {
            var score = Scores.Find(s => s.Player.Id == playerId);
            if(score == null) throw new KeyNotFoundException("Player not found in leaderboard.");

            score.Player.Name = newName;
        }

        public int GetPlayerScore(int playerId)
        {
            var score = Scores.Find(s => s.Player.Id == playerId);
            return score?.Score ?? 0;
        }
    }
}
