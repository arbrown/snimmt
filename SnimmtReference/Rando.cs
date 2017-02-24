using SnimmtPlugin;
using SnimmtGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnimmtGame.Events;

namespace SnimmtReference
{
    public class Rando : ISnimmtPlayer
    {
        public string Name => "Rando";

        public GameState State { get; set; }

        private StringBuilder log { get; set; } = new StringBuilder();

        private Random rand { get; set; } = new Random();
        public IList<Card> Hand { get; set; }

        public Rando()
        {
            Hand = new List<Card>();
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

        public void ReceiveGameState(GameState gameState)
        {
            this.State = gameState;
        }

        public void SetHand(Hand hand)
        {
            Hand = hand.Cards.ToList();
        }

        public void Register(EventManager eventManager)
        {

            // Just register for an event
            eventManager.Register<PlayerCardEvent>(pce =>
            {
                Console.WriteLine($"Today's Lottery Numbers are {pce.Card.Number}, {pce.Card.BullValue}, and {rand.Next()}");
                log.Append($"I observed \"{pce.Player} take the card \"{pce.Card}\"\"");
                });

        }
    }
}
