using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public class GameState
    {
        public ICollection<Player> Players => Game.Players;
        public ICollection<Pile> Piles => Game.Piles;
        public ShuffleStrategy ShuffleStrategy => Game.ShuffleStrategy;

        private Game Game { get; set; }

        public GameState(Game game)
        {
            Game = game;
        }
    }
}
