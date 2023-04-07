using Kitchen;

namespace ChallengeMod.Events
{
    public class ServeCustomerEventArgs : GameEventArgs
    {
        public CPlayer player;
        public CItem item;
        public CCustomerGroup customerGroup;
        public CWaitingForItem waitingForItem;
        public CTableSet tableSet;
    }
}
