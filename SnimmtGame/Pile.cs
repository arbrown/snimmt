using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public class Pile
    {
        public IList<Card> Cards => cards.Select(c => c).ToList();
        public int BullValue => Cards.Sum(c => c.BullValue);

        private IList<Card> cards;

        public Pile()
        {
            cards = new List<Card>();
        }

        public Pile(Card card) : this()
        {
            cards.Add(card);
        }

        internal void AddCard(Card card)
        {
            if (Cards.Count >= 5)
            {
                throw new InvalidOperationException("Can not add card to full pile");
            }

            var top = Cards.Last();

            if (top.Number >= card.Number)
            {
                throw new ArgumentException("Invalid card for this pile.");
            }

            cards.Add(card);
        }

        internal IList<Card> ReplacePile(Card c)
        {
            var oldCards = cards;
            cards = new List<Card>();
            cards.Add(c);

            return oldCards;
        }

        public override string ToString() => $"Pile: [{string.Join(",", cards)}]";
    }
}
