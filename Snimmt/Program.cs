using SnimmtGame;
using SnimmtPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Snimmt
{
    class Program
    {
        private static bool ListAvailableAIs { get; set; } = false;

        private static IList<string> SelectedAIs { get; set; }

        private static int GamesToPlay { get; set; } = 1;

        private static int Verbosity { get; set; } = 0;

        private static string DllPath { get; set; } = "./";




        static void Main(string[] args)
        {
            ParseCommandLineOptions(args);

            var AiDllLoader = new AiDllLoader() { SearchPath = DllPath};

            ISnimmtPlayer ai1, ai2;
            AiDllLoader.TryGetAi("Rando", out ai1);
            AiDllLoader.TryGetAi("Rando", out ai2);

            var game = new Game() { EventDispatcher = new EventDispatcher() };

            var aiDict = new Dictionary<Player, ISnimmtPlayer>();
            var p1 = new Player($"{ai1} 1");
            var p2 = new Player($"{ai2} 2");
            aiDict.Add(p1, ai1);
            ai1.Register(game.EventDispatcher);
            aiDict.Add(p2, ai2);
            ai2.Register(game.EventDispatcher);

            game.AddPlayer(p1);
            game.AddPlayer(p2);

            var gameState = new GameState(game);

            // Give them their hands and a reference to the game state
            foreach (var player in game.Players)
            {
                var ai = aiDict[player];
                ai.ReceiveGameState(gameState);
                var hand = game.GetPlayerHand(player);
                ai.SetHand(hand);
            }

            // TODO: move game logic to game class

            // Real game loop
            // Just play a round
            for (var i = 0; i < 10; i++)
            {
                var playerCards = new Dictionary<Player,Card>();
                foreach (var player in game.Players)
                {
                    var ai = aiDict[player];
                    var card = ai.PlayCard();
                    playerCards[player] = card;
                }

                //Now play each card in order
                foreach (var pc in playerCards.OrderBy(kvp => kvp.Value.Number))
                {
                    Pile pile = null;
                    if (!game.TryPlayCard(pc.Value, pc.Key, out pile))
                    {
                        // Play didn't work... must take a pile
                        if (pile == null)
                        {
                            var ai = aiDict[pc.Key];
                            pile = ai.PickPile();
                        }
                        game.TakePile(pc.Value, pc.Key, pile);
                    }
                }
            }

            game.ScoreRound();

        }

        private static void ParseCommandLineOptions(string[] args)
        {
            var exe = AppDomain.CurrentDomain.FriendlyName;
            for (var i = 0; i<args.Length; i++)
            {
                var arg = args[i];
                switch (arg)
                {
                    case "-l":
                    case "--list":
                        ListAvailableAIs = true;
                        break;

                    case "-v":
                    case "--verbose":
                        Verbosity = 1;
                        if (args.Length > i+1)
                        {
                            //peek to see if verbosity specified
                            var nextArg = args[i + 1];
                            int verbosityLevel;
                            if (int.TryParse(nextArg, out verbosityLevel))
                            {
                                Verbosity = verbosityLevel;
                                i++;
                            }
                        }
                        break;
                    case "-g":
                    case "--games":
                        if (args.Length > i+1)
                        {
                            var nextArg = args[i + 1];
                            int games;
                            if (int.TryParse(nextArg, out games))
                            {
                                GamesToPlay = games;
                                i++;
                            }
                        }
                        break;
                    case "-ai":
                        SelectedAIs = new List<string>();
                        while (args.Length > i+1)
                        {
                            var nextArg = args[i + 1];
                            if (!nextArg.StartsWith("-"))
                            {
                                SelectedAIs.Add(nextArg);
                                i++;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;
                    case "--path":
                    case "-p":
                        if (args.Length > i + 1)
                        {
                            var nextArg = args[i + 1];
                            if (Directory.Exists(nextArg))
                            {
                                DllPath = nextArg;
                                i++;
                            }
                        }
                        break;


                    default:
                        continue;
                }
            }
        }
    }
}
