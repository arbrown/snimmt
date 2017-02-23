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
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Receive the GameState object from the game control
        /// </summary>
        /// <param name="gameState">The game state object that can be used to observe the game</param>
        void RegisterGameState(GameState gameState);

        /// <summary>
        /// Receive a hand from the game control
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

        /// <summary>
        /// Observe the fact that a player played a card
        /// </summary>
        /// <param name="player">The player who just flipped over a card</param>
        /// <param name="card">The card the player played</param>
        void ObservePlayerCard(Player player, Card card);
        // Maybe this would be better as a callback that the plugin registers either during RegisterGameState or a completely different Register method...
        // Especially if there are other potential 'events' an AI might want to know, but are not included in here yet...  That way I don't need
        // to burden down the interface with a bunch of unnecessary methods...




    }
}
