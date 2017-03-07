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

        #region CL args
        private static bool ListAvailableAIs { get; set; } = false;

        private static IList<string> SelectedAIs { get; set; } = new List<string>();

        private static int GamesToPlay { get; set; } = 1;

        private static int Verbosity { get; set; } = 0;

        private static string DllPath { get; set; } = "./";
        #endregion

        static void Main(string[] args)
        {
            ParseCommandLineOptions(args);

            var AiDllLoader = new AiDllLoader(DllPath);

            if (ListAvailableAIs)
            {
                var aiNames = AiDllLoader.GetNames();

                Console.WriteLine("Available AIs:");
                foreach(var name in aiNames)
                {
                    Console.WriteLine(name);
                }
                Environment.Exit(0);
            }

            if (SelectedAIs.Count == 0)
            {
                Console.WriteLine("No AIs selected!");
                Environment.Exit(1);
            }

            var aiDict = new Dictionary<Player, ISnimmtPlayer>();
            var game = new Game() { EventDispatcher = new EventDispatcher() };

            foreach (var name in SelectedAIs)
            {
                if (AiDllLoader.TryGetAi(name, out ISnimmtPlayer ai))
                {
                    var player = new Player(name);
                    aiDict.Add(player, ai);
                    ai.Register(game.EventDispatcher);
                    game.AddPlayer(player);
                }
            }
            var gameState = new GameState(game);

            // Give them their hands and a reference to the game state
            foreach (var player in game.Players)
            {
                if (aiDict.TryGetValue(player, out ISnimmtPlayer ai))
                {
                    ai.ReceiveGameState(gameState);
                    var hand = game.GetPlayerHand(player);
                    ai.SetHand(hand);
                }
            }

            // TODO: move game logic to game class

            // Real game loop
            // Play a game
            var gamesPlayed = 0;
            while (gamesPlayed < GamesToPlay)
            {
                // Play 10 cards in a round
                for (var i = 0; i < 10; i++)
                {
                    var playerCards = new Dictionary<Player, Card>();
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
                if (game.ScoreRound())
                {
                    gamesPlayed++;
                    if (Verbosity > 0)
                    {
                        Console.WriteLine("Game Finished");
                        Console.WriteLine("Final Scores:");
                        Console.WriteLine(string.Format("|{0,10}|{1,7}|", "Player", "Score"));
                        foreach (var player in game.Players.OrderBy(p => p.Score))
                        {
                            Console.WriteLine(string.Format("|{0,10}|{1,7}|", player.Name, player.Score));
                        }
                    }
                }
                else
                {
                    //deal new hands!
                    foreach (var player in game.Players)
                    {
                        game.DealPlayerHand(player);
                        if (aiDict.TryGetValue(player, out ISnimmtPlayer ai))
                        {
                            var hand = game.GetPlayerHand(player);
                            ai.SetHand(hand);
                        }
                    }
                }
            } 

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
                                break;
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
