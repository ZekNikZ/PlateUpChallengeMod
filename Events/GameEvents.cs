using Kitchen;
using System;
using System.Collections.Generic;

namespace ChallengeMod.Events
{
    public class GameEventArgs
    {
        public long Timestamp = DateTime.Now.Ticks;
    }

    public class GameEventArgsWithEntityContext : GameEventArgs
    {
        public EntityContext Context;
    }

    public delegate void GameEventHandler<T>(T args) where T : GameEventArgs;

    public class GameEvents
    {
        public static event GameEventHandler<ItemTransferEventArgs> ItemTransfer;
        public static event GameEventHandler<PlayerGrabItemEventArgs> PlayerGrabItem;
        public static event GameEventHandler<PlayerDropItemEventArgs> PlayerDropItem;

        private static readonly Dictionary<Type, object> Events = new();

        internal static void Init()
        {
            Events.Add(typeof(ItemTransferEventArgs), ItemTransfer);
            Events.Add(typeof(PlayerDropItemEventArgs), PlayerGrabItem);
            Events.Add(typeof(PlayerDropItemEventArgs), PlayerDropItem);
        }

        internal static void Raise<T>(T args) where T : GameEventArgs { 
            if (Events.ContainsKey(typeof(T)))
            {
                (Events[typeof(T)] as GameEventHandler<T>)?.Invoke(args);
            }
        }
    }
}
