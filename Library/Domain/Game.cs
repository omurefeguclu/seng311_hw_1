using System;

namespace Library.Domain
{
    public class Game
    {
        public Player Player { get; set; }
        public DateTime PlayedAt { get; set; }
        public int Score { get; set; }
    }
}