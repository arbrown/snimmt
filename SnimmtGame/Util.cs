using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public static class Util
    {
        public static int BullValue(int number)
        {
            if (number == 55) return 7;
            if (number % 11 == 0) return 5;
            if (number % 10 == 0) return 3;
            if (number % 5 == 0) return 2;
            if (number <= 104 && number >= 1) return 1;
            throw new ArgumentException("6 Nimmt! numbers must be between 1 and 104.");

        }

        //Fisher-Yates Shuffle
        public static void Shuffle(this IList<Card> deck)
        {
            var rand = new Random();

            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        public static Card Draw(this IList<Card> deck)
        {
            var i = deck.Count - 1;
            var card = deck[i];
            deck.RemoveAt(i);
            return card;
        }
    }
}
