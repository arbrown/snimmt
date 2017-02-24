namespace SnimmtGame.Events
{
    public class PlayerTookPileEvent : IEvent
    {
        /// <summary>
        /// The player who took the pile
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The Pile that was taken
        /// </summary>
        public Pile Pile { get; set; }

        /// <summary>
        /// The new card starting the new pile
        /// </summary>
        public Card NewCard { get; set; }
    }
}