using Kitchen;
using System;
using System.Collections.Generic;

namespace ChallengeMod.Events
{
    public abstract class GameEvent
    {
        public long Timestamp = DateTime.Now.Ticks;
        private EntityContext? _context = null;
        public EntityContext Context
        {
            get { return _context ?? Mod.EntityManager; }
            set { _context = value; }
        }

        public abstract override string ToString();
    }

    public delegate void GameEventHandler<T>(T args) where T : GameEvent;

    public class GameEvents
    {
        private static readonly Dictionary<Type, List<Delegate>> Events = new();

        internal static void AddEvent<T>() where T : GameEvent
        {
            Events.Add(typeof(T), new());
            Mod.LogInfo($"Registered event {typeof(T).Name}");

            if (Mod.DEBUG_MODE)
            {
                Subscribe<T>(args =>
                {
                    Mod.LogInfo($"Event fired: {typeof(T).Name} - {args}");
                });
            }
        }

        internal static void Subscribe<T>(GameEventHandler<T> handler) where T : GameEvent
        {
            Events[typeof(T)].Add(handler);
        }

        internal static void Init()
        {
            AddEvent<ItemTransferEvent>();
            AddEvent<PlayerGrabItemEvent>();
            AddEvent<PlayerPlaceItemEvent>();
            AddEvent<AutomationGrabItemEvent>();
            AddEvent<AutomationPlaceItemEvent>();
        }

        internal static void Raise<T>(T args) where T : GameEvent
        {
            if (Events.ContainsKey(typeof(T)))
            {
                foreach (var handler in Events[typeof(T)])
                {
                    handler.DynamicInvoke(args);
                }
            }
            else
            {
                Mod.LogWarning($"Unknown event raised: {typeof(T).Name}");
            }
        }
    }
}
