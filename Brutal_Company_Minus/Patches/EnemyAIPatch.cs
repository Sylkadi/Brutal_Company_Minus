using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Brutal_Company_Minus;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.MeetsStandardPlayerCollisionConditions))] 
        public static void OnMeetsStandardPlayerCollisionConditions(ref PlayerControllerB __result, ref Collider other, ref EnemyType ___enemyType, ref bool ___isEnemyDead, ref bool inKillAnimation, ref float ___stunNormalizedTimer) // This fix works, maybe theres a better way
        {
            PlayerControllerB controller = other.gameObject.GetComponent<PlayerControllerB>();
            if (controller != null)
            {
                if (!___isEnemyDead && ___stunNormalizedTimer < 0.0f && !inKillAnimation && __result == null) // (This may have some unintended consequences)
                {
                    if (controller.actualClientId == GameNetworkManager.Instance.localPlayerController.actualClientId) __result = controller;
                    /*
                    if (___enemyType.name == "SpringMan")
                    {
                        if (__result.IsServer && !__result.IsOwner)
                        {
                            __result = controller;
                            // Damage player
                            Server.Instance.ForcePlayerDamageServerRpc(__result.actualClientId, 90, true, true, CauseOfDeath.Mauling, 2);

                            // Apply fear
                            if (!(1 - __result.playersManager.fearLevel < 0.05f))
                            {
                                __result.playersManager.fearLevel = 1;
                                __result.playersManager.fearLevelIncreasing = true;
                            }
                        }
                    }
                    */
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemyServerRpc))]
        static void OnKillEnemyServerRpc()
        {
            if (!Plugin.BountyActive) return;
            Plugin.Paycut += UnityEngine.Random.Range(5 + (Plugin.DaysPassed / 7), 20 + (Plugin.DaysPassed / 5));
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.Start))]
        static void OnStart(EnemyAI __instance) // Set isOutside
        {
            try
            {
                GameObject terrainMap = GameObject.FindGameObjectWithTag(Plugin.TerrainTag);
                GameObject[] objects = GameObject.FindGameObjectsWithTag(Plugin.TerrainTag);
                foreach(GameObject obj in objects)
                {
                    if(obj.name == Plugin.TerrainName)
                    {
                        terrainMap = obj;
                    }
                }
                
                if(__instance.transform.position.y > terrainMap.transform.position.y - 100)
                {
                    __instance.isOutside = true;
                    __instance.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode"); // Otherwise AI would be fucked
                    if (GameNetworkManager.Instance.localPlayerController != null)
                    {
                        __instance.EnableEnemyMesh(!StartOfRound.Instance.hangarDoorsClosed || !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom);
                    }
                    __instance.SyncPositionToClients();
                }
                else
                {
                    __instance.isOutside = false;
                    __instance.allAINodes = GameObject.FindGameObjectsWithTag("AINode");
                    __instance.SyncPositionToClients();
                }
            } catch
            {
                Log.LogError("Failed to set isOutside on EnemyAI.Start");
            }
        }
    }
}
