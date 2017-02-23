using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    class Pile
    {
        public IList<Card> Cards { get; }
        public int BullValue => Cards.Sum(c => c.BullValue);

        public Pile()
        {
            Cards = new List<Card>();
        }

        public void AddCard(Card card)
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

            Cards.Add(card);
        }
    }
}
