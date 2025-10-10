using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Domain
{
    public class GameStore
    {
        private readonly List<Game> _games = new();

        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        public IReadOnlyCollection<Game> GetAllGames()
        {
            return _games.AsReadOnly();
        }

        public List<Game> GetGamesByPlayer(int playerId)
        {
            return _games.Where(g => g.Player.Id == playerId).ToList();
        }

        public List<Game> GetGamesByDate(DateTime date)
        {
            return _games.Where(g => g.PlayedAt.Date == date.Date).ToList();
        }
    }
}
