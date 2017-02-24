namespace SnimmtGame.Events
{
    public class PlayerCardEvent : IEvent
    {
        /// <summary>
        /// The player that took a card
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The card that was taken
        /// </summary>
        public Card Card { get; set; }
    }
}