using System.Linq;
using Library.Domain;

namespace Library.Helpers
{
    public static class UserProfileHelper
    {
        public static UserProfileModel GetUserProfile(int playerId, GameStore gameStore, LeaderboardManager leaderboardManager, int lastGamesCount = 10)
        {
            if(playerId <= 0)
            {
                throw new ArgumentException("Player ID must be a positive integer.", nameof(playerId));
            }
            
            if(lastGamesCount <= 0)
            {
                throw new ArgumentException("Last games count must be a positive integer.", nameof(lastGamesCount));
            }
            
            var games = gameStore.GetGamesByPlayer(playerId)
                .OrderByDescending(g => g.PlayedAt)
                .Take(lastGamesCount)
                .ToList();

            var scores = leaderboardManager.GetAllScoresForPlayer(playerId);
            if(scores.Count == 0)
            {
                scores = new List<LeaderboardScoreOverview>();
            }
            
            var playerName = games.FirstOrDefault()?.Player?.Name;
            if (string.IsNullOrEmpty(playerName))
                throw new KeyNotFoundException("Player not found in game store.");

            return new UserProfileModel
            {
                PlayerId = playerId,
                PlayerName = playerName,
                LastGames = games,
                LeaderboardScores = scores
            };
        }
    }
}