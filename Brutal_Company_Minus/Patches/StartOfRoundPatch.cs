using System;
using System.Collections.Generic;
using System.Text;
using Brutal_Company_Minus.EventUI;
using HarmonyLib;
using Unity.Netcode;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        public static void OnStart()
        {
            Server.SpawnServerObject();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.EndGameServerRpc))]
        public static void OnEndGameServerRpc()
        {
            // Randomize weather multipliers
            Server.Instance.UpdateCurrentWeatherMultipliersServerRpc();

            // Reset enemy spawns to old
            RoundManager.Instance.currentLevel.Enemies.Clear();
            RoundManager.Instance.currentLevel.OutsideEnemies.Clear();
            RoundManager.Instance.currentLevel.DaytimeEnemies.Clear();

            RoundManager.Instance.currentLevel.Enemies.AddRange(Plugin.insideEnemies);
            RoundManager.Instance.currentLevel.OutsideEnemies.AddRange(Plugin.outsideEnemies);
            RoundManager.Instance.currentLevel.DaytimeEnemies.AddRange(Plugin.daytimeEnemies);

            // If called on server
            if (RoundManager.Instance.IsServer) {
                Server.Instance.outsideObjectsToSpawn.Clear();
                UI.ClearText();
            }
        }
    }
}
