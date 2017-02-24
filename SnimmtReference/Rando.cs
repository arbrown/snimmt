using SnimmtPlugin; // Plugin interface
using SnimmtGame; // Game objects
using SnimmtGame.Events; // Game events
using System; // Writing to console
using System.Collections.Generic; // Generic lists and functions
using System.Linq; // Linq extension methods (Sum, ToList, Select, etc...)
using System.Text; // StringBuilder for log example


namespace SnimmtReference
{
    /// <summary>
    /// An example AI that inherits from ISnimmtPlayer.
    /// 
    /// Rando isn't very good at 6 Nimmt!, but it does
    /// a good job of showing how to implement an AI 
    /// that might be.
    /// </summary>
    public class Rando : ISnimmtPlayer
    {
        /// <summary>
        /// This is the first thing any AI needs: a name.
        /// 
        /// It must be ready to provide its name immediately 
        /// upon instantiation.  A get-only property works great.
        /// </summary>
        public string Name => "Rando";

        /// <summary>
        /// Here is where Rando keeps the variables
        /// and collections that help him do his thing.
        /// </summary>
        #region Class Variables
        private GameState State { get; set; }
        private StringBuilder log { get; set; }
        private Random rand { get; set; } 
        private IList<Card> Hand { get; set; } 
        #endregion

        /// <summary>
        /// This is the default (paramterless) constructor
        /// that will get called when the game finds an AI.
        /// 
        /// Note that there is nothing here.  Constructors should
        /// be as minimal as possible, saving anything memory or
        /// computationally expensive for 'Register' which will be
        /// called when it is guarnateed the AI will be playing a game.
        /// </summary>
        public Rando()
        {
        }

        /// <summary>
        /// This is the method that gets called when an AI is being summoned 
        /// to play a game. Its purpose is two-fold: to allow the AI to perform
        /// any perparations it needs in order to play, and also to allow the AI
        /// to register to listen to certain game events with the game event manager.
        /// </summary>
        /// <param name="eventManager">
        /// The EventManager allows an AI to register callbacks to perform
        /// when certain game events occur (for instance, a player reveals a card, etc...)
        /// </param>
        public void Register(EventManager eventManager)
        {
            // Rando makes his preparations, in this case, preparing a random number generator
            // and a log where he writes his observations about the game.
            this.rand = new Random();
            this.log = new StringBuilder();


            // Rando also registers a callback for the PlayerCardEvent.  When a player plays a card,
            // Rando's function will run.  In this case, Rando prints a snarky message to the console
            // and writes in his log about what he just observed.
            eventManager.Register<PlayerCardEvent>(pce =>
            {
                Console.WriteLine($"Today's Lottery Numbers are {pce.Card.Number}, {pce.Card.BullValue}, and {rand.Next()}");
                log.Append($"I observed \"{pce.Player} take the card \"{pce.Card}\"\"");
            });
        }

        /// <summary>
        /// Right before a game starts, Rando receives a GameState reference that will allow him
        /// to track the official game state (participants, scores, public knowledge, etc...)
        /// Rando stores this game state object since it won't be passed again.
        /// </summary>
        /// <param name="gameState"></param>
        public void ReceiveGameState(GameState gameState)
        {
            this.State = gameState;
        }

        /// <summary>
        /// Once the game has started (but before any rounds), the game manager wil 
        /// notify Rando what his in initial hand is.
        /// </summary>
        /// <param name="hand">The hand Rando was dealt</param>
        public void SetHand(Hand hand)
        {
            Hand = hand.Cards.ToList();
        }

        /// <summary>
        /// The main decision Rando must make is which card to play given a game
        /// state (and any history he remembers.)
        /// </summary>
        /// <returns>A card from his had that Rando wishes to play face down.</returns>
        public Card PlayCard()
        {
            var i = rand.Next(Hand.Count);
            var card = Hand[i];
            Hand.RemoveAt(i);
            return card;
        }

        /// <summary>
        /// During the course of the game, Rando may need to collect a pile of cards
        /// (either because his card was too low to play or it was the 6th card on a pile)
        /// in this case, the game manager will ask Rando to pick a pile to collect.
        /// 
        /// Note that Rando has access to the actual game Piles in the GameState
        /// </summary>
        /// <returns>The Pile object Rando wishes to collect</returns>
        public Pile PickPile()
        {
            var r = rand.Next(4);
            return State.Piles.ToList()[r];
        }

        // And that's it!  There is nothing more to implement for a snimmt AI!

    }
}
