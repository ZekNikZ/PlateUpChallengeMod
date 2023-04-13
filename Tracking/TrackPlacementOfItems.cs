using ChallengeMod.Events;
using Kitchen;
using KitchenMods;

namespace ChallengeMod.Tracking
{
    internal class TrackPlacementOfItems : GenericSystemBase, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();

            GameEvents.Subscribe<PlayerPlaceItemEvent>(args =>
            {
                if (args.DestinationType != TransferTarget.Holder) return;

                args.Context.Set(args.Item, new CPlacedBy
                {
                    Source = args.Player,
                    SourceType = CPlacedBy.ItemSource.Player,
                    Timestamp = args.Timestamp
                });
            });

            GameEvents.Subscribe<AutomationPlaceItemEvent>(args =>
            {
                if (args.DestinationType != TransferTarget.Holder) return;

                args.Context.Set(args.Item, new CPlacedBy
                {
                    Source = args.Source,
                    SourceType = CPlacedBy.ItemSource.Automation,
                    Timestamp = args.Timestamp
                });
            });
        }

        protected override void OnUpdate() { }
    }
}
