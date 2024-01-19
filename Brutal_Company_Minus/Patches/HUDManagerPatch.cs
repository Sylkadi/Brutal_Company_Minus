using System;
using System.Collections.Generic;
using System.Text;
using Brutal_Company_Minus.EventUI;
using GameNetcodeStuff;
using HarmonyLib;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        public static void OnUpdate(ref PlayerControllerB ___localPlayer)
        {
            try
            {
                OpenCloseUI.Instance.keyPressEnabledTyping = !___localPlayer.isTypingChat;
            }  catch { }
        }
    }
}
