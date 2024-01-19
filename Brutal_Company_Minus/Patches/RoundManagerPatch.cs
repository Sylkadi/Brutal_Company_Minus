using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brutal_Company_Minus;
using Brutal_Company_Minus._Event;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus.Patches
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("FinishGeneratingLevel")]
        public static void OnFinishGeneratingLevel()
        {
            Manager.SampleMap();

            // Client side objects
            Manager.randomSeedValue = StartOfRound.Instance.randomMapSeed + 2; // Reset seed value
            RoundManager.Instance.StartCoroutine(DelayedExecution());

            // Net objects
            foreach (cObj<GameObject> obj in Plugin.insideObjectsToSpawnOutside)
            {
                Manager.SpawnOutsideObjects(obj._object, obj.count, 75.0f, new Vector3(0.0f, -0.05f, 0.0f));
            }
        }

        private static IEnumerator DelayedExecution() // Delay this to fix trees not spawning in correctly on clients
        {
            yield return new WaitForSeconds(5.0f);
            foreach (OutsideObjectsToSpawn obj in Server.Instance.outsideObjectsToSpawn)
            {
                Manager.SpawnOutsideObjects(Lists.ObjectList[(Lists.ObjectName)obj.objectEnumID], obj.density, 100.0f, new Vector3(0.0f, -1.0f, 0.0f));
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch("RefreshEnemyVents")]
        static void OnRefreshEnemyVents()
        {
            if (RoundManager.Instance.allEnemyVents.Length != 0)
            {
                Manager.DoSpawnInsideEnemies();
                Manager.DoSpawnOutsideEnemies();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void OnUpdate()
        {
            if (Plugin.Paycut > 0)
            {
                Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();

                terminal.groupCredits += Plugin.Paycut;
                terminal.SyncGroupCreditsServerRpc(terminal.groupCredits, terminal.numberOfItemsInDropship);

                HUDManager.Instance.AddTextToChatOnServer("<color=green>+" + Plugin.Paycut + "$</color>");

                Plugin.Paycut = 0;
            }

            if (Plugin.DoorGlitchActive)
            {
                TerminalAccessibleObject[] doors = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
                if (doors.Length > 0 && UnityEngine.Random.Range(0, (int)(15 / Time.deltaTime)) == 1)
                {
                    foreach (TerminalAccessibleObject door in doors)
                    {
                        if (UnityEngine.Random.Range(0, 3) == 1)
                        {
                            door.SetDoorOpenServerRpc(true);
                        }
                        else
                        {
                            door.SetDoorOpenServerRpc(false);
                        }
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("waitForScrapToSpawnToSync")]
        static void OnWaitForScrapToSpawnToSync()
        {
            // Scrap Setting Resseting
            RoundManager.Instance.currentLevel.spawnableScrap.Clear();
            RoundManager.Instance.currentLevel.spawnableScrap.AddRange(Plugin.levelScrap);
            RoundManager.Instance.currentLevel.minScrap = Plugin.MinScrap;
            RoundManager.Instance.currentLevel.maxScrap = Plugin.MaxScrap;
        }

    }
}
