using System;
using System.Collections.Generic;
using System.Text;
using Brutal_Company_Minus.EventUI;
using HarmonyLib;
using Unity.Netcode;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void OnStart()
        {
            Server.InitalizeServerObject();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameNetworkManager.StartHost))]
        public static void OnStartHost()
        {
            Plugin.DaysPassed = -1; // Reset Count

            if (Server.Instance != null) Server.Instance.GetComponent<NetworkObject>().Despawn(true);
            Server.doSpawnServerObject = true;
        }
    }
}
