using ChallengeMod.Tracking;
using HarmonyLib;
using Kitchen;
using KitchenData;
using System.Reflection;
using Unity.Entities;

namespace ChallengeMod.Events
{
    public class ServeCustomerEvent : GameEvent
    {
        public enum ItemType
        {
            Dish,
            Side,
            Extra,
            Bonus
        }

        public CPlacedBy? Source; // source
        public Entity Item; // CItem
        public Entity CustomerGroup; // CCustomerGroup
        public CWaitingForItem Request; // request
        public Entity TableSet; // CTableSet
        public ItemType Type;

        public override string ToString()
        {
            return $"{Type} served - {Request.ItemID}";
        }
    }
    [HarmonyPatch]
    internal class GroupReceiveItemReset
    {
        internal static Entity _group;

        static MethodBase TargetMethod()
        {
            var type = AccessTools.FirstInner(typeof(GroupReceiveItem), t => typeof(IJobChunk).IsAssignableFrom(t));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static void Prefix(Entity e)
        {
            _group = e;
        }

        static void Postfix(Entity e)
        {
            GroupRecieveItemSatisfaction._hasBeenSatisfied = false;
        }
    }

    [HarmonyPatch]
    internal class GroupRecieveItemSatisfaction
    {
        private static Entity _item;
        internal static bool _hasBeenSatisfied = false;

        [HarmonyPatch(typeof(GroupReceiveItem), "IsRequestSatisfied")]
        [HarmonyPostfix]
        static void IsRequestSatisfied(GameData data, Entity request, Entity candidate, ref int also_provided_side, ref bool __result)
        {
            if (__result)
            {
                _item = candidate;
                _hasBeenSatisfied = true;
            }
        }

        [HarmonyPatch(typeof(GroupReceiveItem), "Satisfy")]
        [HarmonyPrefix]
        static bool Satisfy(CWaitingForItem satisfied_order, ref DynamicBuffer<CWaitingForItem> all_orders, int satisfy_equivalent_max, out int reward, ref GroupReceiveItem __instance)
        {
            if (!_hasBeenSatisfied)
            {
                reward = 0;
                return true;
            }

            reward = 0;
            int num = 0;
            for (int i = 0; i < all_orders.Length; i++)
            {
                CWaitingForItem cWaitingForItem = all_orders[i];
                bool flag = cWaitingForItem.Item == satisfied_order.Item;
                if (cWaitingForItem.Satisfied)
                {
                    continue;
                }
                if (!flag)
                {
                    if (num >= satisfy_equivalent_max || !__instance.IsEquivalentRequest(cWaitingForItem, satisfied_order))
                    {
                        continue;
                    }
                    num++;
                }
                reward += cWaitingForItem.Reward;
                cWaitingForItem.Satisfied = true;
                all_orders[i] = cWaitingForItem;
                GameEvents.Raise(new ServeCustomerEvent
                {
                    Source = Mod.EntityManager.RequireComponent(_item, out CPlacedBy placedBy) ? placedBy : null,
                    Item = _item,
                    CustomerGroup = GroupReceiveItemReset._group,
                    Request = cWaitingForItem,
                    TableSet = Mod.EntityManager.RequireComponent(_item, out CHeldBy heldBy) ? heldBy.Holder : default,
                    Type = cWaitingForItem.IsSide ? ServeCustomerEvent.ItemType.Side : ServeCustomerEvent.ItemType.Dish
                });
            }

            return false;
        }
    }
}
