using Kitchen;

namespace ChallengeMod.Events
{
    public class ServeCustomerEvent : GameEvent
    {
        public CPlayer player;
        public CItem item;
        public CCustomerGroup customerGroup;
        public CWaitingForItem waitingForItem;
        public CTableSet tableSet;

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}
