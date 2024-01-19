using System;
using System.Collections.Generic;
using System.Text;
using Brutal_Company_Minus.EventUI;
using GameNetcodeStuff;
using HarmonyLib;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        public static void OnUpdate(ref QuickMenuManager ___quickMenuManager)
        {
            try
            {
                OpenCloseUI.Instance.keyPressEnabledSettings = !___quickMenuManager.isMenuOpen;
            } catch { }
        }
    }
}
