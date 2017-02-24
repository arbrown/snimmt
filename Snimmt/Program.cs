using SnimmtGame;
using SnimmtGame.Events;
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
        static void Main(string[] args)
        {
            var dllPaths = Directory.GetFiles(".", "*.dll");

            var dlls = new List<Assembly>();

            foreach (var file in dllPaths)
            {
                var an = AssemblyName.GetAssemblyName(file);
                var dll = Assembly.Load(an);
                dlls.Add(dll);
            }

            var playerType = typeof(ISnimmtPlayer);
            var plugins = new List<Type>();
            foreach (var dll in dlls)
            {
                if (dll != null)
                {
                    var types = dll.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(playerType.FullName) != null)
                            {
                                plugins.Add(type);
                            }
                        }
                    }
                }
            }

            var AIs = new List<ISnimmtPlayer>();

            foreach (var type in plugins)
            {
                // Instantiate plugins here.
                ISnimmtPlayer ai = (ISnimmtPlayer)Activator.CreateInstance(type);
                AIs.Add(ai);
            }


            // For now, just take the first guy, and play it against itself

            var ai1 = (ISnimmtPlayer)Activator.CreateInstance(plugins.First());
            var ai2 = (ISnimmtPlayer)Activator.CreateInstance(plugins.First());

            var game = new Game() { EventManager = new EventManager() };

            var aiDict = new Dictionary<Player, ISnimmtPlayer>();
            var p1 = new Player($"{ai1} 1");
            var p2 = new Player($"{ai2} 2");
            aiDict.Add(p1, ai1);
            ai1.Register(game.EventManager);
            aiDict.Add(p2, ai2);
            ai2.Register(game.EventManager);

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

                //Now tell the players that care
                foreach (var pc in playerCards)
                {
                    game.EventManager.Broadcast(new PlayerCardEvent() { Player = pc.Key, Card = pc.Value });
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
    }
}
