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

            GameEvents.PlayerPlaceItem += args =>
            {
                if (args.DestinationType != TransferTarget.Holder) return;

                args.Context.Set(args.Item, new CPlacedPy
                {
                    Source = args.Player,
                    SourceType = CPlacedPy.ItemSource.Player,
                    Timestamp = args.Timestamp
                });
            };

            GameEvents.AutomationPlaceItem += args =>
            {
                if (args.DestinationType != TransferTarget.Holder) return;

                args.Context.Set(args.Item, new CPlacedPy
                {
                    Source = args.Source,
                    SourceType = CPlacedPy.ItemSource.Automation,
                    Timestamp = args.Timestamp
                });
            };
        }

        protected override void OnUpdate() { }
    }
}
