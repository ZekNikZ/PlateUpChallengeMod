using Unity.Entities;

namespace ChallengeMod.Events
{
    public enum TransferTarget
    {
        Holder,
        Provider,
        Bin
    }

    public class ItemTransferEvent : GameEvent
    {
        public Entity Source;
        public Entity Destination;
        public Entity Item;
        public TransferTarget SourceType;
        public TransferTarget DestinationType;

        public override string ToString()
        {
            return $"{SourceType} => {DestinationType}";
        }
    }

    public interface IPlayerItemTransferEvent
    {
        public Entity Player { get; }
        public Entity Surface { get; }
    }

    public class PlayerGrabItemEvent : ItemTransferEvent, IPlayerItemTransferEvent
    {
        public Entity Player => Destination;
        public Entity Surface => Source;
    }

    public class PlayerPlaceItemEvent : ItemTransferEvent, IPlayerItemTransferEvent
    {
        public Entity Player => Source;
        public Entity Surface => Destination;
    }

    public class AutomationGrabItemEvent : ItemTransferEvent
    {
    }

    public class AutomationPlaceItemEvent : ItemTransferEvent
    {
    }
}
