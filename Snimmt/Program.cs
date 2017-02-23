using SnimmtGame;

namespace Snimmt
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();

            foreach (var player in game.Players)
            {
                var h = game.GetPlayerHand(player);
            }
        }
    }
}
