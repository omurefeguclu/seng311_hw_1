using Library.Domain;

namespace Library
{
    public class LeaderboardManager
    {
        private readonly Dictionary<string, Leaderboard> _leaderboards = new();

        public void AddLeaderboard(string name)
        {
            if (!_leaderboards.ContainsKey(name))
                _leaderboards[name] = new Leaderboard { Name = name };
        }
        
        public Leaderboard? GetLeaderboard(string name)
        {
            return _leaderboards.GetValueOrDefault(name);
        }

        public List<LeaderboardScoreOverview> GetAllScoresForPlayer(int playerId)
        {
            var result = new List<LeaderboardScoreOverview>();
            foreach (var kvp in _leaderboards)
            {
                int score = kvp.Value.GetPlayerScore(playerId);
                result.Add(new LeaderboardScoreOverview
                {
                    LeaderboardName = kvp.Key,
                    PlayerScore = score
                });
            }
            return result;
        }

        public IEnumerable<string> GetLeaderboardNames() => _leaderboards.Keys;

        public Leaderboard GetOrAddLeaderboard(string name)
        {
            if (_leaderboards.TryGetValue(name, out var value)) return value;
            
            value = new Leaderboard { Name = name };
            _leaderboards[name] = value;
            return value;
        }
    }
}
