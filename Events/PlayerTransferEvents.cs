﻿using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace ChallengeMod.Events
{
    [HarmonyPatch(typeof(AcceptIntoHolder), "AcceptTransfer")]
    internal class PlayerTransferHolderPatch
    {
        static void Prefix(Entity proposal_entity, EntityContext ctx)
        {
            if (ctx.Require(proposal_entity, out CItemTransferProposal comp))
            {
                var transferTarget = ctx.Has<CItemProvider>(comp.Source) ? TransferTarget.Provider : TransferTarget.Holder;

                GameEvents.Raise(new ItemTransferEventArgs
                {
                    Source = comp.Source,
                    Destination = comp.Destination,
                    Item = comp.Item,
                    Context = ctx,
                    SourceType = transferTarget,
                    DestinationType = TransferTarget.Holder
                });

                if (ctx.Has<CPlayer>(comp.Destination))
                {
                    GameEvents.Raise(new PlayerGrabItemEventArgs
                    {
                        Source = comp.Source,
                        Destination = comp.Destination,
                        Item = comp.Item,
                        Context = ctx,
                        SourceType = transferTarget,
                        DestinationType = TransferTarget.Holder
                    });
                }
                else if (ctx.Has<CPlayer>(comp.Source))
                {
                    GameEvents.Raise(new PlayerDropItemEventArgs
                    {
                        Source = comp.Source,
                        Destination = comp.Destination,
                        Item = comp.Item,
                        Context = ctx,
                        SourceType = transferTarget,
                        DestinationType = TransferTarget.Holder
                    });
                }
            }
        }
    }

    [HarmonyPatch(typeof(AcceptIntoProvider), "AcceptTransfer")]
    internal class PlayerTransferIntoProviderPatch
    {
        static void Prefix(Entity proposal_entity, EntityContext ctx)
        {
            if (ctx.Require(proposal_entity, out CItemTransferProposal comp) && ctx.Require(comp.Destination, out CItemProvider comp2))
            {
                var transferTarget = ctx.Has<CItemProvider>(comp.Source) ? TransferTarget.Provider : TransferTarget.Holder;

                GameEvents.Raise(new ItemTransferEventArgs
                {
                    Source = comp.Source,
                    Destination = comp.Destination,
                    Item = comp.Item,
                    Context = ctx,
                    SourceType = transferTarget,
                    DestinationType = TransferTarget.Provider
                });

                if (ctx.Has<CPlayer>(comp.Source))
                {
                    GameEvents.Raise(new PlayerDropItemEventArgs
                    {
                        Source = comp.Source,
                        Destination = comp.Destination,
                        Item = comp.Item,
                        Context = ctx,
                        SourceType = transferTarget,
                        DestinationType = TransferTarget.Provider
                    });
                }
            }
        }
    }

    [HarmonyPatch(typeof(TakeFromProvider), "SendTransfer")]
    internal class PlayerTransferFromProviderPatch
    {
        static void Prefix(Entity transfer, EntityContext ctx)
        {
            if (ctx.Require(transfer, out CItemTransferProposal comp) && ctx.Require(comp.Source, out CItemProvider comp2) && comp2.Maximum > 0)
            {
                if (ctx.Has<CPlayer>(comp.Destination))
                {
                    GameEvents.Raise(new PlayerGrabItemEventArgs
                    {
                        Source = comp.Source,
                        Destination = comp.Destination,
                        Item = comp.Item,
                        Context = ctx,
                        SourceType = TransferTarget.Provider,
                        DestinationType = TransferTarget.Holder
                    });
                }
            }
        }
    }
}
