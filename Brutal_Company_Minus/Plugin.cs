using BepInEx;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using Brutal_Company_Minus._Event;
using BepInEx.Configuration;
using System.Linq;
using Brutal_Company_Minus.Patches;
using GameNetcodeStuff;
using System.Security.Cryptography;
using UnityEngine.Experimental.AI;
using System;
using Events = Brutal_Company_Minus._Event.Events;
using System.Collections;
using Brutal_Company_Minus.EventUI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Brutal_Company_Minus
{
    [HarmonyPatch]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "Drinkable.Brutal_Company_Minus";
        private const string NAME = "Brutal_Company_Minus";
        private const string VERSION = "0.6.6";
        
        public static Plugin Instance;
        private readonly Harmony harmony = new Harmony(GUID);
        
        public static List<cObj<EnemyType>> enemiesToSpawnInside = new List<cObj<EnemyType>>();
        public static List<cObj<EnemyType>> enemiesToSpawnOutside = new List<cObj<EnemyType>>();
        public static List<cObj<GameObject>> insideObjectsToSpawnOutside = new List<cObj<GameObject>>();

        public static int randomItemsToSpawnOutsideCount = 0;

        public static List<Event> events = new List<Event>() {
            // Very Good
            new Events.BigBonus(),
            new Events.ScrapGalore(),
            new Events.GoldenBars(),
            new Events.BigDelivery(),
            new Events.PlentyOutsideScrap(),
            // Good
            new Events.Bounty(),
            new Events.Bonus(),
            new Events.SmallerMap(),
            new Events.MoreScrap(),
            new Events.HigherScrapValue(),
            new Events.GoldenFacility(),
            new Events.Dentures(),
            new Events.Pickles(),
            new Events.Honk(),
            new Events.TransmuteScrapSmall(),
            new Events.SmallDeilvery(),
            new Events.ScarceOutsideScrap(),
            // Neutral
            new Events.Nothing(),
            new Events.Locusts(),
            new Events.Birds(),
            new Events.Trees(),
            new Events.LeaflessBrownTrees(),
            new Events.LeaflessTrees(),
            // Bad
            new Events.HoardingBugs(),
            new Events.Bees(),
            new Events.Landmines(),
            new Events.Lizard(),
            new Events.Slimes(),
            new Events.Thumpers(),
            new Events.Turrets(),
            new Events.Spiders(),
            new Events.SnareFleas(),
            new Events.DoorGlitch(),
            new Events.OutsideTurrets(),
            new Events.OutsideLandmines(),
            // Very Bad
            new Events.Nutcracker(),
            new Events.Arachnophobia(),
            new Events.Bracken(),
            new Events.Coilhead(),
            new Events.BaboonHorde(),
            new Events.Dogs(),
            new Events.Jester(),
            new Events.LittleGirl(),
            new Events.AntiCoilhead(),
            new Events.ChineseProduce(),
            new Events.TransmuteScrapBig(),
            new Events.Warzone(),
            new Events.GypsyColony(),
            new Events.ForestGiant(),
            new Events.InsideBees(),
            // NoEnemy
            new Events.NoBaboons(),
            new Events.NoBracken(),
            new Events.NoCoilhead(),
            new Events.NoDogs(),
            new Events.NoGiants(),
            new Events.NoHoardingBugs(),
            new Events.NoJester(),
            new Events.NoLittleGirl(),
            new Events.NoLizards(),
            new Events.NoNutcracker(),
            new Events.NoSpiders(),
            new Events.NoThumpers(),
            new Events.NoSnareFleas(),
            new Events.NoWorm(),
            new Events.NoSlimes(),
            new Events.NoMasks(),
            new Events.NoTurrets(),
            new Events.NoLandmines()
        };
        public static List<Event> disabledEvents = new List<Event>();

        public static List<SpawnableItemWithRarity> levelScrap = new List<SpawnableItemWithRarity>();
        public static int MinScrap = 0, MaxScrap = 0;

        public static List<SpawnableEnemyWithRarity> insideEnemies = new List<SpawnableEnemyWithRarity>();
        public static List<SpawnableEnemyWithRarity> outsideEnemies = new List<SpawnableEnemyWithRarity>();
        public static List<SpawnableEnemyWithRarity> daytimeEnemies = new List<SpawnableEnemyWithRarity>();

        public static float TerrainArea = 0.0f;
        public static string TerrainTag = "";
        public static string TerrainName = "";

        public static List<GameObject> objectsToClear = new List<GameObject>();

        public static bool BountyActive = false, DoorGlitchActive = false;
        public static int Paycut = 0, DaysPassed = -1;

        public static float factorySizeMultiplier = 1f, scrapValueMultiplier = 0.4f, scrapAmountMultiplier = 1f;
        public static int scrapMinAmount = 0, scrapMaxAmount = 0;

        public static AssetBundle bundle;

        void Awake()
        {
            if (Instance == null) Instance = this;
            // Required for netcode... dont remove...
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }      

            // Log
            Log.Initalize(Logger);

            // Generate Lists
            Lists.GenerateLists();

            // Load asset bundle
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Brutal_Company_Minus.asset")) bundle = AssetBundle.LoadFromStream(stream);

            // Initialize Events
            foreach (Event e in events) e.Initalize();
            
            // Config
            Configuration.Initalize();
            Manager.UpdateAllEventWeights();

            // Use config settings
            for (int i = 0; i != events.Count; i++)
            {
                if (Configuration.useCustomWeights.Value) events[i].Weight = Configuration.eventWeights[i].Value;
                events[i].Description = Configuration.eventDescriptions[i].Value;
                events[i].ColorHex = Configuration.eventColorHexes[i].Value;
                events[i].Type = Configuration.eventTypes[i].Value;
                events[i].ScaleList = Configuration.eventScales[i];
                events[i].Enabled = Configuration.eventEnables[i].Value;
            }

            // Create disabled events list and update
            List<Event> newEvents = new List<Event>();
            foreach(Event e in events)
            {
                if (!e.Enabled)
                {
                    disabledEvents.Add(e);
                } else
                {
                    newEvents.Add(e);
                }
            }
            events = newEvents;

            // Harmony patch all
            harmony.PatchAll();

            Log.LogInfo(NAME + " " + VERSION + "   " + "is Done Patching.");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        static void ModifyLevel(ref SelectableLevel newLevel)
        {
            DaysPassed++;

            Server.Instance.ClearGameObjectsClientRpc(); // Clear all previously placed objects on all clients
            if (!RoundManager.Instance.IsHost || newLevel.levelID == 3) return;

            // Reset values
            ResetValues(newLevel);

            // Apply weather multipliers
            foreach (Weather e in Server.Instance.currentWeatherMultipliers)
            {
                if(newLevel.currentWeather == e.weatherType)
                {
                    scrapValueMultiplier *= e.scrapValueMultiplier;
                    scrapAmountMultiplier *= e.scrapAmountMultiplier;
                    factorySizeMultiplier *= e.factorySizeMultiplier;
                }
            }

            // Update Enemy max power counts
            RoundManager.Instance.currentMaxInsidePower += DaysPassed;
            RoundManager.Instance.currentMaxOutsidePower += DaysPassed / 2;

            // Apply events
            List<Event> additionalEvents = new List<Event>();
            List<Event> currentEvents = Manager.ChooseEvents(newLevel, events, out additionalEvents);
            Manager.ApplyEvents(currentEvents);
            Manager.ApplyEvents(additionalEvents);

            // Log Chosen events
            foreach (Event e in currentEvents) Log.LogInfo("Event chosen: " + e.Name());

            // Spawn outside scrap
            Manager.DoSpawnScrapOutside(1.0f, randomItemsToSpawnOutsideCount);

            // Sync values to all clients
            Server.Instance.SetMultipliersClientRpc(factorySizeMultiplier, scrapValueMultiplier, scrapAmountMultiplier);

            // Apply UI
            UI.GenerateText(currentEvents);

            // Logging
            Log.LogInfo("MapMultipliers = [scrapValueMultiplier: " + scrapValueMultiplier + ",     scrapAmountMultiplier: " + scrapAmountMultiplier + ",     factorySizeMultiplier:" + factorySizeMultiplier + "]");
            Log.LogInfo("IsAntiCoildHead = " + Server.Instance.isAntiCoilHead.Value);
        }
        
        static void ResetValues(SelectableLevel newLevel)
        {
            foreach (SpawnableMapObject obj in newLevel.spawnableMapObjects)
            {
                if (obj.prefabToSpawn.name == "Landmine") obj.numberToSpawn = new AnimationCurve(new Keyframe(0, 2.5f));
                if (obj.prefabToSpawn.name == "TurretContainer") obj.numberToSpawn = new AnimationCurve(new Keyframe(0, 2.5f));
            }
            BountyActive = false; DoorGlitchActive = false; Server.Instance.isAntiCoilHead.Value = false;

            levelScrap.Clear();
            levelScrap.AddRange(newLevel.spawnableScrap);
            MinScrap = newLevel.minScrap;
            MaxScrap = newLevel.maxScrap;

            insideEnemies.Clear();
            outsideEnemies.Clear();
            daytimeEnemies.Clear();
            insideEnemies.AddRange(newLevel.Enemies);
            outsideEnemies.AddRange(newLevel.OutsideEnemies);
            daytimeEnemies.AddRange(newLevel.DaytimeEnemies);

            // Reset Multipliers
            try {
                factorySizeMultiplier = Lists.factorySizeMultiplierList[newLevel.levelID];
            } catch {
                factorySizeMultiplier = 1f;
            }
            scrapAmountMultiplier = 1.0f;
            scrapValueMultiplier = 0.4f; // Default value is 0.4 not 1.0
            scrapMinAmount = MinScrap;
            scrapMaxAmount = MaxScrap;
            randomItemsToSpawnOutsideCount = 0;

            // Reset objectSpawnLists
            insideObjectsToSpawnOutside.Clear();
        }
    }
}
