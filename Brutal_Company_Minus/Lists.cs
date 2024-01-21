using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Brutal_Company_Minus
{
    internal class Lists
    {

        public enum EnemyName
        {
            Bracken, HoardingBug, CoilHead, Thumper, BunkerSpider, Jester, SnareFlea, Hygrodere, GhostGirl, SporeLizard, NutCracker, Masked, EyelessDog, ForestKeeper, EarthLeviathan, BaboonHawk, RoamingLocust, Manticoil, CircuitBee, Lasso
        }
        public static Dictionary<EnemyName, EnemyType> EnemyList = new Dictionary<EnemyName, EnemyType>();

        public enum ItemName
        {
            LargeAxle, V_TypeEngine, PlasticFish, MetalSheet, LaserPointer, BigBolt, Bottles, Ring, SteeringWheel, CookieMoldPan, EggBeater, JarOfPickles, DustPan, AirHorn, ClownHorn, CashRegister, Candy, GoldBar, YieldSign, HomemadeFlashbang, Gift, Flask, ToyCube, Remote, ToyRobot, MagnifyingGlass, StopSign, TeaKettle, Mug, RedSoda, OldPhone, HairDryer, Brush, Bell, WhoopieCushion, Comedy, Tragedy, RubberDucky, ChemicalJug, FancyLamp, GoldenCup, Painting, Toothpaste, PillBottle, PerfumeBottle, Teeth, Magic7Ball
        }
        public static Dictionary<ItemName, Item> ItemList = new Dictionary<ItemName, Item>();
        public enum ObjectName
        {
            LargeRock1, LargeRock2, LargeRock3, LargeRock4, TreeLeaflessBrown1, GiantPumkin, GreyRockGrouping2, GreyRockGrouping4, Tree, TreeLeafless2, TreeLeafless3, Landmine, Turret
        }
        public static Dictionary<ObjectName, GameObject> ObjectList = new Dictionary<ObjectName, GameObject>();

        public static List<float> factorySizeMultiplierList = new List<float>();

        private static bool generatedList = false;
        public static void GenerateLists()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (!generatedList && StartOfRound.Instance != null)
                {
                    // Generate enemyList
                    List<EnemyType> AllEnemies = Resources.FindObjectsOfTypeAll<EnemyType>().Concat(GameObject.FindObjectsByType<EnemyType>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToList();
                    AllEnemies = AllEnemies.GroupBy(x => x.name).Select(x => x.FirstOrDefault()).OrderBy(x => x.name).ToList(); // Remove duplicates if exists

                    foreach (EnemyType enemy in AllEnemies)
                    {
                        switch (enemy.name)
                        {
                            case "Centipede":
                                EnemyList.Add(EnemyName.SnareFlea, enemy);
                                break;
                            case "SandSpider":
                                EnemyList.Add(EnemyName.BunkerSpider, enemy);
                                break;
                            case "HoarderBug":
                                EnemyList.Add(EnemyName.HoardingBug, enemy);
                                break;
                            case "Flowerman":
                                EnemyList.Add(EnemyName.Bracken, enemy);
                                break;
                            case "Crawler":
                                EnemyList.Add(EnemyName.Thumper, enemy);
                                break;
                            case "Blob":
                                EnemyList.Add(EnemyName.Hygrodere, enemy);
                                break;
                            case "DressGirl":
                                EnemyList.Add(EnemyName.GhostGirl, enemy);
                                break;
                            case "Puffer":
                                EnemyList.Add(EnemyName.SporeLizard, enemy);
                                break;
                            case "Nutcracker":
                                EnemyList.Add(EnemyName.NutCracker, enemy);
                                break;
                            case "MouthDog":
                                EnemyList.Add(EnemyName.EyelessDog, enemy);
                                break;
                            case "ForestGiant":
                                EnemyList.Add(EnemyName.ForestKeeper, enemy);
                                break;
                            case "SandWorm":
                                EnemyList.Add(EnemyName.EarthLeviathan, enemy);
                                break;
                            case "RedLocustBees":
                                EnemyList.Add(EnemyName.CircuitBee, enemy);
                                break;
                            case "Doublewing":
                                EnemyList.Add(EnemyName.Manticoil, enemy);
                                break;
                            case "DocileLocustBees":
                                EnemyList.Add(EnemyName.RoamingLocust, enemy);
                                break;
                            case "BaboonHawk":
                                EnemyList.Add(EnemyName.BaboonHawk, enemy);
                                break;
                            case "SpringMan":
                                EnemyList.Add(EnemyName.CoilHead, enemy);
                                break;
                            case "Jester":
                                EnemyList.Add(EnemyName.Jester, enemy);
                                break;
                            case "LassoMan":
                                EnemyList.Add(EnemyName.Lasso, enemy);
                                break;
                            case "MaskedPlayerEnemy":
                                EnemyList.Add(EnemyName.Masked, enemy);
                                break;
                        }
                    }

                    // Generate itemList
                    List<Item> AllItems = Resources.FindObjectsOfTypeAll<Item>().Concat(GameObject.FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToList();
                    AllItems = AllItems.GroupBy(x => x.name).Select(x => x.FirstOrDefault()).OrderBy(x => x.name).ToList(); // Remove duplicates if exists

                    foreach (Item item in AllItems)
                    {
                        switch (item.name)
                        {
                            case "Cog1":
                                ItemList.Add(ItemName.LargeAxle, item);
                                break;
                            case "EnginePart1":
                                ItemList.Add(ItemName.V_TypeEngine, item);
                                break;
                            case "FishTestProp":
                                ItemList.Add(ItemName.PlasticFish, item);
                                break;
                            case "MetalSheet":
                                ItemList.Add(ItemName.MetalSheet, item);
                                break;
                            case "FlashLaserPointer":
                                ItemList.Add(ItemName.LaserPointer, item);
                                break;
                            case "BigBolt":
                                ItemList.Add(ItemName.BigBolt, item);
                                break;
                            case "BottleBin":
                                ItemList.Add(ItemName.Bottles, item);
                                break;
                            case "Ring":
                                ItemList.Add(ItemName.Ring, item);
                                break;
                            case "SteeringWheel":
                                ItemList.Add(ItemName.SteeringWheel, item);
                                break;
                            case "MoldPan":
                                ItemList.Add(ItemName.CookieMoldPan, item);
                                break;
                            case "EggBeater":
                                ItemList.Add(ItemName.EggBeater, item);
                                break;
                            case "PickleJar":
                                ItemList.Add(ItemName.JarOfPickles, item);
                                break;
                            case "DustPan":
                                ItemList.Add(ItemName.DustPan, item);
                                break;
                            case "Airhorn":
                                ItemList.Add(ItemName.AirHorn, item);
                                break;
                            case "ClownHorn":
                                ItemList.Add(ItemName.ClownHorn, item);
                                break;
                            case "CashRegister":
                                ItemList.Add(ItemName.CashRegister, item);
                                break;
                            case "Candy":
                                ItemList.Add(ItemName.Candy, item);
                                break;
                            case "GoldBar":
                                ItemList.Add(ItemName.GoldBar, item);
                                break;
                            case "YieldSign":
                                ItemList.Add(ItemName.YieldSign, item);
                                break;
                            case "DiyFlashbang":
                                ItemList.Add(ItemName.HomemadeFlashbang, item);
                                break;
                            case "GiftBox":
                                ItemList.Add(ItemName.Gift, item);
                                break;
                            case "Flask":
                                ItemList.Add(ItemName.Flask, item);
                                break;
                            case "ToyCube":
                                ItemList.Add(ItemName.ToyCube, item);
                                break;
                            case "Remote":
                                ItemList.Add(ItemName.Remote, item);
                                break;
                            case "RobotToy":
                                ItemList.Add(ItemName.ToyRobot, item);
                                break;
                            case "MagnifyingGlass":
                                ItemList.Add(ItemName.MagnifyingGlass, item);
                                break;
                            case "StopSign":
                                ItemList.Add(ItemName.StopSign, item);
                                break;
                            case "TeaKettle":
                                ItemList.Add(ItemName.TeaKettle, item);
                                break;
                            case "Mug":
                                ItemList.Add(ItemName.Mug, item);
                                break;
                            case "SodaCanRed":
                                ItemList.Add(ItemName.RedSoda, item);
                                break;
                            case "Phone":
                                ItemList.Add(ItemName.OldPhone, item);
                                break;
                            case "Hairdryer":
                                ItemList.Add(ItemName.HairDryer, item);
                                break;
                            case "Brush":
                                ItemList.Add(ItemName.Brush, item);
                                break;
                            case "Bell":
                                ItemList.Add(ItemName.Bell, item);
                                break;
                            case "WhoopieCushion":
                                ItemList.Add(ItemName.WhoopieCushion, item);
                                break;
                            case "ComedyMask":
                                ItemList.Add(ItemName.Comedy, item);
                                break;
                            case "TragedyMask":
                                ItemList.Add(ItemName.Tragedy, item);
                                break;
                            case "RubberDuck":
                                ItemList.Add(ItemName.RubberDucky, item);
                                break;
                            case "ChemicalJug":
                                ItemList.Add(ItemName.ChemicalJug, item);
                                break;
                            case "FancyLamp":
                                ItemList.Add(ItemName.FancyLamp, item);
                                break;
                            case "FancyCup":
                                ItemList.Add(ItemName.GoldenCup, item);
                                break;
                            case "FancyPainting":
                                ItemList.Add(ItemName.Painting, item);
                                break;
                            case "Toothpaste":
                                ItemList.Add(ItemName.Toothpaste, item);
                                break;
                            case "PillBottle":
                                ItemList.Add(ItemName.PillBottle, item);
                                break;
                            case "PerfumeBottle":
                                ItemList.Add(ItemName.PerfumeBottle, item);
                                break;
                            case "Dentures":
                                ItemList.Add(ItemName.Teeth, item);
                                break;
                            case "7Ball":
                                ItemList.Add(ItemName.Magic7Ball, item);
                                break;
                        }
                    }

                    // Generate objectList
                    List<SpawnableMapObject> insideObjectList = new List<SpawnableMapObject>();
                    List<SpawnableOutsideObjectWithRarity> outsideObjectList = new List<SpawnableOutsideObjectWithRarity>();

                    foreach (SelectableLevel level in StartOfRound.Instance.levels)
                    {
                        foreach (SpawnableMapObject obj in level.spawnableMapObjects)
                        {
                            if (insideObjectList.FindIndex(o => o.prefabToSpawn.name == obj.prefabToSpawn.name) < 0) // If dosent exist in list then add
                            {
                                insideObjectList.Add(obj);
                            }
                        }

                        foreach (SpawnableOutsideObjectWithRarity obj in level.spawnableOutsideObjects)
                        {
                            if (outsideObjectList.FindIndex(o => o.spawnableObject.prefabToSpawn.name == obj.spawnableObject.prefabToSpawn.name) < 0) // If dosent exist in list then add
                            {
                                outsideObjectList.Add(obj);
                            }
                        }
                    }

                    foreach (SpawnableMapObject obj in insideObjectList)
                    {
                        switch (obj.prefabToSpawn.name)
                        {
                            case "Landmine":
                                ObjectList.Add(ObjectName.Landmine, obj.prefabToSpawn);
                                break;
                            case "TurretContainer":
                                ObjectList.Add(ObjectName.Turret, obj.prefabToSpawn);
                                break;
                        }
                    }

                    foreach (SpawnableOutsideObjectWithRarity obj in outsideObjectList)
                    {
                        switch (obj.spawnableObject.prefabToSpawn.name)
                        {
                            case "LargeRock1":
                                ObjectList.Add(ObjectName.LargeRock1, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "LargeRock2":
                                ObjectList.Add(ObjectName.LargeRock2, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "LargeRock3":
                                ObjectList.Add(ObjectName.LargeRock3, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "LargeRock4":
                                ObjectList.Add(ObjectName.LargeRock4, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "GreyRockGrouping2":
                                ObjectList.Add(ObjectName.GreyRockGrouping2, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "GreyRockGrouping4":
                                ObjectList.Add(ObjectName.GreyRockGrouping4, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "GiantPumpkin":
                                ObjectList.Add(ObjectName.GiantPumkin, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "tree":
                                ObjectList.Add(ObjectName.Tree, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "treeLeaflessBrown.001 Variant":
                                ObjectList.Add(ObjectName.TreeLeaflessBrown1, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "treeLeafless.002_LOD0":
                                ObjectList.Add(ObjectName.TreeLeafless2, obj.spawnableObject.prefabToSpawn);
                                break;
                            case "treeLeafless.003_LOD0":
                                ObjectList.Add(ObjectName.TreeLeafless3, obj.spawnableObject.prefabToSpawn);
                                break;
                        }
                    }

                    // Generate Factory size multiplier list
                    foreach (SelectableLevel level in StartOfRound.Instance.levels)
                    {
                        factorySizeMultiplierList.Add(level.factorySizeMultiplier);
                    }
                    
                    generatedList = true;
                }
            };
        }
    }
}
