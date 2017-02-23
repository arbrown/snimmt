using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public class Hand
    {
        public ICollection<Card> Cards { get; private set; }

        public Hand()
        {
            Cards = new HashSet<Card>();
        }

        public Hand(ICollection<Card> cards)
        {
            Cards = cards;
        }

        public Hand(Hand hand)
        {
            Cards = hand.Cards.Select(c => c).ToList();
        }

        public Card RemoveCard(Card card)
        {
            var ok = Cards.Contains(card);

            if (!ok) throw new ArgumentException($"Hand does not contain card {card}.");

            Cards.Remove(card);
            return card;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }




        
    }
}
