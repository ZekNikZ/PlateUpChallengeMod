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
        public static event GameEventHandler<PlayerPlaceItemEventArgs> PlayerPlaceItem;
        public static event GameEventHandler<AutomationGrabItemEventArgs> AutomationGrabItem;
        public static event GameEventHandler<AutomationPlaceItemEventArgs> AutomationPlaceItem;

        private static readonly Dictionary<Type, object> Events = new();

        internal static void Init()
        {
            // Events
            Events.Add(typeof(ItemTransferEventArgs), ItemTransfer);
            Events.Add(typeof(PlayerPlaceItemEventArgs), PlayerGrabItem);
            Events.Add(typeof(PlayerPlaceItemEventArgs), PlayerPlaceItem);
            Events.Add(typeof(AutomationGrabItemEventArgs), AutomationGrabItem);
            Events.Add(typeof(AutomationPlaceItemEventArgs), AutomationPlaceItem);
        }

        internal static void Raise<T>(T args) where T : GameEventArgs { 
            if (Events.ContainsKey(typeof(T)))
            {
                (Events[typeof(T)] as GameEventHandler<T>)?.Invoke(args);
            }
        }
    }
}
