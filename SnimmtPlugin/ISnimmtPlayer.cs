using SnimmtGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtPlugin
{
    public interface ISnimmtPlayer
    {
        /// <summary>
        /// Returns the name of this Snimmt player
        /// This property should be able to identify
        /// an AI before any other preparation /
        /// initialization has been done in Register()
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Register for the game.
        /// This is the function in which any memory allocation or 
        /// general preparations should occur.  The constructor
        /// should be pretty much empty.
        /// </summary>
        /// <param name="eventDispatcher">Event Dispatcher object used to register for events</param>
        void Register(EventDispatcher eventDispatcher);

        /// <summary>
        /// Receive the GameState object from the game control.
        /// This object will be used to track the game as it proceeds, and
        /// will be passed after all other AIs have registered for the game.
        /// </summary>
        /// <param name="gameState">The game state object that can be used to observe the game</param>
        void ReceiveGameState(GameState gameState);

        /// <summary>
        /// Receive the starting hand from the game control
        /// </summary>
        /// <param name="hand">The player's new hand</param>
        void SetHand(Hand hand);

        /// <summary>
        /// Given the current game state, play a card
        /// </summary>
        /// <returns>The card to be played</returns>
        Card PlayCard();

        /// <summary>
        /// Given the current game state, the player must pick a pile to claim
        /// </summary>
        /// <returns>The chosen pile</returns>
        Pile PickPile();

    }
}
