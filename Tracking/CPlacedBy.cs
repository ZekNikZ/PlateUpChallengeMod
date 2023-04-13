using KitchenMods;
using Unity.Entities;

namespace ChallengeMod.Tracking
{
    public struct CPlacedBy : IModComponent
    {
        public enum ItemSource {
            Player,
            Automation
        }

        public Entity Source;
        public ItemSource SourceType;
        public long Timestamp;
    }
}
