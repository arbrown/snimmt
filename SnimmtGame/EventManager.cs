﻿using SnimmtGame.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    public class EventDispatcher
    {

        private MultiValueDictionary<Type, Action<IEvent>> Events { get; set; } = new MultiValueDictionary<Type, Action<IEvent>>();

        public void Register<T>(Action<T> action) where T:IEvent
        {
            var type = typeof(T);
            var castAction = new Action<IEvent>(e => action((T)e));
            Events.Add(type, castAction);
        }

        internal void Broadcast<T>(T e) where T:IEvent
        {
            var type = typeof(T);
            IReadOnlyCollection<Action<IEvent>> actions;

            if (Events.TryGetValue(type, out actions))
            {
                foreach (var action in actions)
                {
                    action(e);
                }
            }
        }
    }
}
