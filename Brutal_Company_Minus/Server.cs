using Brutal_Company_Minus;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using Brutal_Company_Minus.EventUI;

namespace Brutal_Company_Minus
{
    internal class Server : NetworkBehaviour
    {
        public static Server Instance { get; private set; }
        public static GameObject serverObject { get; private set; }

        public static bool doSpawnServerObject = true;

        public NetworkList<Weather> currentWeatherMultipliers;
        public NetworkList<OutsideObjectsToSpawn> outsideObjectsToSpawn;

        public NetworkVariable<ulong> playerToForciblyDamage = new NetworkVariable<ulong>();
        public NetworkVariable<bool> isAntiCoilHead = new NetworkVariable<bool>();

        public NetworkVariable<FixedString4096Bytes> textUI = new NetworkVariable<FixedString4096Bytes>();

        void Awake()
        {
            currentWeatherMultipliers = new NetworkList<Weather>();
            outsideObjectsToSpawn = new NetworkList<OutsideObjectsToSpawn>();
        }

        public override void OnNetworkSpawn()
        {
            Instance = this;

            UI.SpawnObject(); // Spawn client side UI object

            if (IsServer) { // Only call on server
                playerToForciblyDamage.Value = 0;
                isAntiCoilHead.Value = false;
                InitalizeCurrentWeatherMultipliersServerRpc();
            }

            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            // Remove UI
            OpenCloseUI.Instance.UnsubscribeFromKeyboardEvent();
            Destroy(GameObject.Find("EventGUI"));

            base.OnNetworkDespawn();
        }

        public static void InitalizeServerObject()
        {
            if (serverObject != null) return;

            serverObject = (GameObject)Plugin.bundle.LoadAsset("NetworkHandler");
            serverObject.AddComponent<Server>();

            NetworkManager.Singleton.AddNetworkPrefab(serverObject);
        }

        public static void SpawnServerObject()
        {
            if (!doSpawnServerObject || !FindObjectOfType<NetworkManager>().IsServer) return;

            GameObject networkHandler = Instantiate(serverObject, Vector3.zero, Quaternion.identity);
            networkHandler.GetComponent<NetworkObject>().Spawn(destroyWithScene: false);

            doSpawnServerObject = false;
        }


        [ClientRpc]
        public void ClearGameObjectsClientRpc()
        {
            for (int i = 0; i != Plugin.objectsToClear.Count; i++)
            {
                if (Plugin.objectsToClear[i] != null)
                {
                    NetworkObject netObject = Plugin.objectsToClear[i].GetComponent<NetworkObject>();

                    if (netObject != null) // If net object
                    {
                        netObject.Despawn(true);
                    }
                    else // If not net object
                    {
                        Destroy(Plugin.objectsToClear[i]);
                    }
                }
            }
            Plugin.objectsToClear.Clear(); // clear list
        }

        [ClientRpc]
        public void SetMultipliersClientRpc(float factorySizeMultiplier, float scrapValueMultiplier, float scrapAmountMultiplier)
        {
            RoundManager.Instance.currentLevel.factorySizeMultiplier = factorySizeMultiplier;
            RoundManager.Instance.scrapValueMultiplier = scrapValueMultiplier;
            RoundManager.Instance.scrapAmountMultiplier = scrapAmountMultiplier;
            RoundManager.Instance.currentLevel.minScrap = Plugin.scrapMinAmount;
            RoundManager.Instance.currentLevel.maxScrap = Plugin.scrapMaxAmount;
        }

        [ServerRpc(RequireOwnership = false)]
        private void InitalizeCurrentWeatherMultipliersServerRpc()
        {
            currentWeatherMultipliers = Weather.InitalizeWeatherMultipliers(currentWeatherMultipliers);
            UpdateCurrentWeatherMultipliersServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateCurrentWeatherMultipliersServerRpc()
        {
            currentWeatherMultipliers = Weather.RandomizeWeatherMultipliers(currentWeatherMultipliers);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ForcePlayerDamageServerRpc(ulong playerID, int damageNumber, bool hasDamageSFX = true, bool callRPC = true, CauseOfDeath causeOfDeath = CauseOfDeath.Unknown, int deathAnimation = 0, bool fallDamage = false, Vector3 force = default(Vector3))
        {
            playerToForciblyDamage.Value = playerID;

            ForcePlayerDamageClientRpc(damageNumber, hasDamageSFX, callRPC, causeOfDeath, deathAnimation, fallDamage);
        }

        [ClientRpc]
        private void ForcePlayerDamageClientRpc(int damageNumber, bool hasDamageSFX = true, bool callRPC = true, CauseOfDeath causeOfDeath = CauseOfDeath.Unknown, int deathAnimation = 0, bool fallDamage = false, Vector3 force = default(Vector3))
        {
            foreach (GameObject obj in StartOfRound.Instance.allPlayerObjects.ToList())
            {
                PlayerControllerB p = obj.GetComponent<PlayerControllerB>();
                if (p.actualClientId == playerToForciblyDamage.Value) p.DamagePlayer(damageNumber, hasDamageSFX, callRPC, causeOfDeath, deathAnimation, fallDamage);
            }
        }

        [ClientRpc]
        public void ShowCaseEventsClientRpc()
        {
            // Showcase Events
            UI.Instance.curretShowCaseEventTime = UI.Instance.showCaseEventTime;
            UI.Instance.panelBackground.SetActive(true); // Show text
            UI.Instance.panelScrollBar.value = 1.0f; // Start from top
            UI.Instance.showCaseEvents = true;
        }
    }
}
