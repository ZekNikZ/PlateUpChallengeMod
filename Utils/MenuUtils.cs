using Kitchen;
using KitchenLib;
using KitchenLib.Utils;
using System.Reflection;

namespace ChallengeMod.Utils
{
    internal static class MenuUtils
    {
        private static MethodInfo mSetPanelTarget = ReflectionUtils.GetMethod<LocalMenuView<PlayerPauseView>>("SetPanelTarget");
        private static FieldInfo fModuleList = ReflectionUtils.GetField<LocalMenuView<PlayerPauseView>>("ModuleList");

        public static void RecomputeSize<T>(this KLMenu<T> menu)
        {
            var comp = menu.Container.parent.parent.GetComponent<LocalMenuView<PlayerPauseView>>();
            mSetPanelTarget.Invoke(comp, new object[] { fModuleList.GetValue(comp) });
            mSetPanelTarget.Invoke(comp, new object[] { menu.ModuleList });
        }
    }
}
