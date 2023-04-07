using Unity.Entities;

namespace ChallengeMod.Events
{
    public enum TransferTarget
    {
        Holder,
        Provider
    }

    public class ItemTransferEventArgs : GameEventArgsWithEntityContext
    {

        public Entity Source;
        public Entity Destination;
        public Entity Item;
        public TransferTarget SourceType;
        public TransferTarget DestinationType;
    }

    public interface IPlayerItemTransferEvent
    {
        public Entity Player { get; }
        public Entity Surface { get; }
    }

    public class PlayerGrabItemEventArgs : ItemTransferEventArgs, IPlayerItemTransferEvent
    {
        public Entity Player => Destination;
        public Entity Surface => Source;
    }

    public class PlayerDropItemEventArgs : ItemTransferEventArgs, IPlayerItemTransferEvent
    {
        public Entity Player => Source;
        public Entity Surface => Destination;
    }

    public class AutomationGrabItemEventArgs : ItemTransferEventArgs
    {
    }

    public class AutomationPlaceItemEventArgs : ItemTransferEventArgs
    {
    }
}
