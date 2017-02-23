using System;
using System.Collections.Generic;
using System.Linq;

namespace SnimmtGame
{
    public class Player
    {
        public string Name { get; private set; }
        public override string ToString() => Name;

        public int Score { get; internal set; }

        // Do not expose actual list
        public ICollection<Card> CardsTakenThisRound => takenCards.Select(c => c).ToList();

        private ICollection<Card> takenCards;

        internal Hand Hand { get; set; }

        public Player()
        {
            takenCards = new List<Card>();
            Hand = new Hand();
        }

        public Player(string name) : this()
        {
            Name = name;
        }
        
        internal void Take(Card card)
        {
            takenCards.Add(card);
        }

        internal ICollection<Card> ScoreRound()
        {
            var bullScore = takenCards.Sum(c => c.BullValue);
            Score += bullScore;
            var retCards = takenCards;
            takenCards = new List<Card>();
            return retCards;
        }
    }
}