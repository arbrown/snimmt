using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    class Hand
    {
        public IEnumerable<Card> Cards { get; private set; }

        public Hand()
        {
            Cards = new HashSet<Card>();
        }

        public Hand(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        public Card RemoveCard(Card card)
        {
            return null;
        }


        
    }
}
