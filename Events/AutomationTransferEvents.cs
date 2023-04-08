using HarmonyLib;
using Kitchen;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace ChallengeMod.Events
{
    [HarmonyPatch]
    internal class GrabItemsPatch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(GrabItems), "AttemptGrabHolder");
            yield return AccessTools.Method(typeof(GrabItems), "AttemptGrabFromProvider");
        }

        static void Postfix(Entity target, EntityContext ctx, Entity e, ref CConveyPushItems grab, ref bool __result)
        {
            if (!__result) return;

            var item = ctx.Require(e, out CItemHolder comp) ? comp.HeldItem : default;

            GameEvents.Raise(new ItemTransferEvent
            {
                Source = target,
                SourceType = ctx.Has<CItemProvider>(target) ? TransferTarget.Provider : TransferTarget.Holder,
                Destination = e,
                DestinationType = TransferTarget.Holder,
                Item = item,
                Context = ctx
            });

            GameEvents.Raise(new AutomationGrabItemEvent
            {
                Source = target,
                SourceType = ctx.Has<CItemProvider>(target) ? TransferTarget.Provider : TransferTarget.Holder,
                Destination = e,
                DestinationType = TransferTarget.Holder,
                Item = item,
                Context = ctx
            });
        }
    }

    [HarmonyPatch]
    internal class PushItemsPatch
    {
        private static bool PreviousHasPerformedAction;
        private static Entity Target;
        private static Entity Item;

        static MethodBase TargetMethod()
        {
            var type = AccessTools.FirstInner(typeof(PushItems), t => typeof(IJobChunk).IsAssignableFrom(t));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static void Prefix(Entity e, ref CConveyPushItems push, ref CItemHolder held, in CPosition pos, ref EntityContext ___ctx, ref bool ___has_performed_action)
        {
            PreviousHasPerformedAction = ___has_performed_action;

            Orientation o = Orientation.Up;
            if (___ctx.Require(e, out CConveyPushRotatable comp) && comp.Target != 0)
            {
                o = comp.Target;
            }

            Vector3 vector = pos.Rotation.RotateOrientation(o).ToOffset() * (!push.Reversed ? 1 : -1);
            Entity occupant = Mod.ZGetOccupant(vector + pos);
            if (Mod.ZCanReach(pos, vector + pos) && !___ctx.Has<CPreventItemTransfer>(occupant))
            {
                Target = occupant;
            }
            else
            {
                Target = default;
            }

            Item = held.HeldItem;
        }

        static void Postfix(Entity e, ref EntityContext ___ctx, ref bool ___has_performed_action)
        {
            if (!PreviousHasPerformedAction && ___has_performed_action && Target != default && Item != default)
            {
                var destinationType = ___ctx.Has<CApplianceBin>(Target) ? TransferTarget.Bin : (___ctx.Has<CItemProvider>(Target) ? TransferTarget.Provider : TransferTarget.Holder);

                GameEvents.Raise(new ItemTransferEvent
                {
                    Source = e,
                    SourceType = TransferTarget.Holder,
                    Destination = Target,
                    DestinationType = destinationType,
                    Item = destinationType == TransferTarget.Holder ? Item : default,
                    Context = ___ctx
                });

                GameEvents.Raise(new AutomationPlaceItemEvent
                {
                    Source = e,
                    SourceType = TransferTarget.Holder,
                    Destination = Target,
                    DestinationType = destinationType,
                    Item = destinationType == TransferTarget.Holder ? Item : default,
                    Context = ___ctx
                });
            }
        }
    }

    [HarmonyPatch]
    internal class TeleportItemsPatch
    {
        private static bool PotentialTeleport = false;

        static MethodBase TargetMethod()
        {
            var type = AccessTools.FirstInner(typeof(TeleportItems), t => typeof(IJobChunk).IsAssignableFrom(t));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static void Prefix(Entity e, ref CItemHolder holder, ref CConveyTeleport teleport)
        {
            PotentialTeleport = Mod.EntityManager.RequireComponent(teleport.Target, out CConveyTeleport otherTeleport) && !otherTeleport.HasReceivedTeleport;
        }

        static void Postfix(Entity e, ref CItemHolder holder, ref CConveyTeleport teleport)
        {
            if (PotentialTeleport && Mod.EntityManager.RequireComponent(teleport.Target, out CConveyTeleport otherTeleport) && otherTeleport.HasReceivedTeleport)
            {
                var item = Mod.EntityManager.RequireComponent(teleport.Target, out CItemHolder newHolder) ? newHolder.HeldItem : default;

                GameEvents.Raise(new ItemTransferEvent
                {
                    Source = e,
                    SourceType = TransferTarget.Holder,
                    Destination = teleport.Target,
                    DestinationType = TransferTarget.Holder,
                    Item = item,
                    Context = Mod.EntityManager
                });

                GameEvents.Raise(new AutomationGrabItemEvent
                {
                    Source = e,
                    SourceType = TransferTarget.Holder,
                    Destination = teleport.Target,
                    DestinationType = TransferTarget.Holder,
                    Item = item,
                    Context = Mod.EntityManager
                });

                GameEvents.Raise(new AutomationPlaceItemEvent
                {
                    Source = e,
                    SourceType = TransferTarget.Holder,
                    Destination = teleport.Target,
                    DestinationType = TransferTarget.Holder,
                    Item = item,
                    Context = Mod.EntityManager
                });
            }
        }
    }
}
