using ChallengeMod.Data;
using ChallengeMod.Events;
using ChallengeMod.UI;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenMods;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace ChallengeMod
{
    internal partial class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "io.zkz.plateup.challenges";
        public const string MOD_NAME = "Challenges";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "ZekNikZ";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        internal static AssetBundle Bundle;
        internal static EntityManager EntityManager;
        internal static Mod Instance;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) {
            Instance = this;
        }

        protected override void OnInitialise()
        {
            EntityManager = base.EntityManager;
            Challenges.Load();
            Challenges.Save();
        }

        private void AddGameData()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");

            LogInfo("Attempting to register game data...");

            LogInfo("Done loading game data.");
        }

        private bool _uiSetup = false;
        protected override void OnUpdate()
        {
            // Setup UI
            if (_uiSetup || UICamera == null) return;
            _uiSetup = true;

        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // Setup events
            GameEvents.Init();

            // Load asset bundle
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();

            // Register challenges
            RegisterChallenges();

            // UI Buttons
            KitchenLib.Event.Events.MainMenu_SetupEvent += (s, args) =>
            {
                args.addSubmenuButton.Invoke(args.instance, new object[] { "Challenges", typeof(ChallengesMenu), false });
            };
            KitchenLib.Event.Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(ChallengesMenu), new ChallengesMenu(args.instance.ButtonContainer, args.module_list) });
            };

            // Perform actions when game data is built
            KitchenLib.Event.Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
            };
        }

        internal static Entity ZGetOccupant(Vector3 position, OccupancyLayer layer = OccupancyLayer.Default)
        {
            return Instance.GetOccupant(position, layer);
        }

        internal static bool ZCanReach(Vector3 from, Vector3 to, bool do_not_swap = false)
        {
            return Instance.CanReach(from, to, do_not_swap);
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
