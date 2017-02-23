using SnimmtPlugin;
using SnimmtGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtReference
{
    public class Rando : ISnimmtPlayer
    {
        public string Name => "Rando";

        public GameState State { get; set; }

        private Random rand { get; set; } = new Random();
        public IList<Card> Hand { get; set; }

        public Rando()
        {
            Hand = new List<Card>();
        }

        public void ObservePlayerCard(Player player, Card card)
        {
            // lol, I don't care!
        }

        public Pile PickPile()
        {
            var r = rand.Next(4);
            return State.Piles.ToList()[r];
        }

        public Card PlayCard()
        {
            var i = rand.Next(Hand.Count);
            var card = Hand[i];
            Hand.RemoveAt(i);
            return card;
        }

        public void RegisterGameState(GameState gameState)
        {
            this.State = gameState;
        }

        public void SetHand(Hand hand)
        {
            Hand = hand.Cards.ToList();
        }
    }
}
