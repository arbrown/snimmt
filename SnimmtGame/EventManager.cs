using SnimmtGame.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public class EventManager
    {

        private MultiValueDictionary<Type, Action<IEvent>> Events { get; set; } = new MultiValueDictionary<Type, Action<IEvent>>();



        private ICollection<Action<PlayerCardEvent>> PlayerCardCallbacks {get; set;} = new HashSet<Action<PlayerCardEvent>>();
        private ICollection<Action<PlayerTookPileEvent>> PlayerTookPileCallbacks { get; set; } = new HashSet<Action<PlayerTookPileEvent>>();


        public EventManager()
        {

        }

        public void Register<T>(Action<T> action) where T:IEvent
        {
            var type = typeof(T);
            var castAction = new Action<IEvent>(e => action((T)e));
            Events.Add(type, castAction);
        }

        public void Broadcast<T>(T e) where T:IEvent
        {
            var type = typeof(T);
            var actions = Events[type];

            foreach (var action in actions)
            {
                action(e);
            }
        }


        public void RegisterPlayerCardCallback(Action<PlayerCardEvent> callback) => PlayerCardCallbacks.Add(callback);
        public void RegisterPlayerTookPileCallback(Action<PlayerTookPileEvent> callback) => PlayerTookPileCallbacks.Add(callback);

        internal void BroadcastPlayerCardCallbacks(Player player, Card card)
        {
            var e = new PlayerCardEvent { Player = player, Card = card };
            foreach (var action in PlayerCardCallbacks)
            {
                action(e);
            }
        }

        internal void BroadcastPlayerTookPileCallbacks(Player player, Card card)
        {
            var e = new PlayerCardEvent { Player = player, Card = card };
            foreach (var action in PlayerCardCallbacks)
            {
                action(e);
            }
        }


    }
}
