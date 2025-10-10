using System.Linq;
using Library.Domain;

namespace Library.Helpers
{
    public static class UserProfileHelper
    {
        public static UserProfileModel GetUserProfile(int playerId, GameStore gameStore, LeaderboardManager leaderboardManager, int lastGamesCount = 10)
        {
            var games = gameStore.GetGamesByPlayer(playerId)
                .OrderByDescending(g => g.PlayedAt)
                .Take(lastGamesCount)
                .ToList();

            var scores = leaderboardManager.GetAllScoresForPlayer(playerId);

            var playerName = games.FirstOrDefault()?.Player?.Name ?? string.Empty;

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