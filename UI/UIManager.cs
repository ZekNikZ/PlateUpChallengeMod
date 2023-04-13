using KitchenLib.Utils;
using TMPro;
using UnityEngine;

namespace ChallengeMod.UI
{
    internal class UIManager
    {
        internal static GameObject ChallengeUI { get; private set; }
        internal static bool ChallengeUIVisible = false;

        public static void Init(AssetBundle bundle, Camera uiCamera)
        {
            var uiParent = uiCamera.transform.Find("UI Container").transform;

            Mod.LogInfo("Loading UI...");

            // Challenge UI
            ChallengeUI = Object.Instantiate(bundle.LoadAsset<GameObject>("ChallengeUI"), uiParent);
            ChallengeUI.AddComponent<ChallengeUIController>();
            Mod.LogInfo("Loaded Challenge UI");

            Mod.LogInfo("Done loading UI");
        }

        public static void OpenChallengeUI()
        {
            ChallengeUIVisible = true;
            Mod.LogInfo("Opening Challenge UI...");
        }

        public static void CloseChallengeUI()
        {
            ChallengeUIVisible = false;
            Mod.LogInfo("Closing Challenge UI...");
        }
    }
}
