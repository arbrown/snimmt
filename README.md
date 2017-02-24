# snimmt

snimmt is a framework for testing AIs that play [6 Nimmt!](https://en.wikipedia.org/wiki/6_Nimmt!)

To implement an AI, create a c# class library with a class that implements the interface [ISnimmtPlayer](..//master/SnimmtPlugin/ISnimmtPlayer.cs).

First, an AI is responsible for reporting its name (via the `Name` property).

Then, if chosen for participation in a game, it will have `Register` called, and given a chance to initialize itself and register for various game events that may be interesting to it (player revealed a card, took a pile, etc...)
To better manage memory and runtime, any memory allocation or pre-processing should be done here, rather than in the constructor.

![6 Nimmt Game](https://upload.wikimedia.org/wikipedia/commons/thumb/5/56/6_nimmt%21.jpg/450px-6_nimmt%21.jpg)

Next, once all AIs are included in the game, `ReceiveGameState` will be called, and the AI will receive an object that will track the state of the game (scores, piles, and other public information.)
Additional preparations or tweaks to AI logic can be performed here.

Before the game starts, the game manager will give each AI a starting hand by calling `SetHand`.  This information (along with publicly available information in the game state) are all that the AI is provided in order to make its decisions.  The rest is up to the AI author.

Finally, in game, the AI only needs to respond to two calls `PlayCard` given the current game state, and if the played card forces a pile to be picked up `PickPile` to pick a pile (from the game state object) to collect.

The game manager searches its directory for dlls containing implementations of `ISnimmtPlayer`, thus dragging a dll to that folder is sufficient to enable that AI.

A [reference implementation](../master/SnimmtReference/Rando.cs) is provided as an example for how to implement a snimmt AI.
