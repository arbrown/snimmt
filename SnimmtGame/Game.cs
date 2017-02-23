using SnimmtGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnimmtGame
{
    public class Game
    {
        public ICollection<Player> Players { get; set; }
        public IList<Pile> Piles { get; set; }
        public IList<Card> Deck { get; set; }

        public ShuffleStrategy ShuffleStrategy { get; set; } = ShuffleStrategy.CycleCardsToBottom;

        public Game()
        {
            Players = new List<Player>();
            Piles = new List<Pile>();

            Deck = FreshDeck();
            
            foreach (var i in Enumerable.Range(1, 4))
            {
                Piles.Add(new Pile(Deck.Draw()));
            }

        }

        private IList<Card> FreshDeck()
        {
            var deck = new List<Card>();

            foreach (Card c in Enumerable.Range(1, 104).Select(i => new Card(i)))
            {
                deck.Add(c);
            }

            deck.Shuffle();
            return deck;
        }

        // Add a player and deal him in off the top of the deck.
        public void AddPlayer(Player p)
        {
            foreach (var i in Enumerable.Range(1, 10))
            {
                p.Hand.AddCard(Deck.Draw());
            }
        }

        //returns true if play valid, false if play impossible (must take pile instead)
        public bool TryPlayCard(Card c, Player p)
        {
            //check if valid
            if (!p.Hand.Cards.Contains(c))
            {
                throw new InvalidOperationException($"Player \"{p}\" does not have card \"{c}\".");
            }


            //find pile
            var delta = 105;
            Pile targetPile = null;
            foreach(var pile in Piles)
            {
                var diff = c.Number - pile.Cards.Last().Number;
                if (diff > 0 && diff < delta)
                {
                    targetPile = pile;
                    delta = diff;
                }
            }
            if (targetPile == null)
            {
                return false;
            }

            else
            {
                try
                {
                    targetPile.AddCard(c);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }

            p.Hand.Cards.Remove(c);
            return true;
        }

        //Take a pile for a player
        public void TakePile(Card c, Player pl, Pile pi)
        {
            // Card must be smaller than all piles' top card
            foreach (var pile in Piles)
            {
                if (pile.Cards.Last().Number < c.Number && pile.Cards.Count < 5)
                {
                    throw new InvalidOperationException("Can not take pile if card can be played.");
                }
            }
                        
            var takenPile = pi.ReplacePile(c);
            foreach (var card in takenPile)
            {
                pl.Take(card);
            }
        }

        // Get player hand to expose it to AI when necessary
        public Hand GetPlayerHand(Player p)
        {
            return new Hand(p.Hand);
        }

        //return true if the game is over
        public bool ScoreRound()
        {
            var cardsToShuffle = new List<Card>();
            foreach (var player in Players)
            {
                var cardsTaken = player.ScoreRound();
                foreach (var card in cardsTaken)
                {
                    cardsToShuffle.Add(card);
                }
            }
            var maxScore = Players.Max(p => p.Score);

            switch (ShuffleStrategy)
            {
                case ShuffleStrategy.CycleCardsToBottom:
                    cardsToShuffle.Shuffle();
                    foreach(var card in cardsToShuffle)
                    {
                        Deck.Add(card);
                    }
                    break;
                case ShuffleStrategy.ShuffleEntireDeck:
                    Deck = FreshDeck();
                    break;
            }

            return maxScore >= 66;

        }



    }
}