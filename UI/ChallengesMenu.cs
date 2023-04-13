using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Registry;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ChallengeMod.UI
{
    public partial class ChallengesMenu : KLMenu<PauseMenuAction>
    {
        public ChallengesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id)
        {
            RequestAction(PauseMenuAction.CloseMenu);
            UIManager.OpenChallengeUI();
        }
    }
}