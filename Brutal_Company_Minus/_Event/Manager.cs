using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using static Brutal_Company_Minus.Configuration;

namespace Brutal_Company_Minus._Event
{
    internal class Manager
    {

        public static int randomSeedValue = 0;
        public static void SpawnOutsideObjects(GameObject obj, float density, float radius, Vector3 offset)
        {
            if (obj == null) return;

            System.Random random = new System.Random(randomSeedValue);

            List<Vector3> spawnDenialPoints = Functions.GetSpawnDenialNodes();

            int count = (int)(density * Plugin.TerrainArea); // Compute amount
            for (int i = 0; i < count; i++)
            {
                randomSeedValue++;

                GameObject Node = RoundManager.Instance.outsideAINodes[random.Next(0, RoundManager.Instance.outsideAINodes.Length)];
                UnityEngine.Random.InitState(randomSeedValue); // Important or wont be same on all clients
                Vector3 position = RoundManager.Instance.GetRandomNavMeshPositionInRadius(Node.transform.position, radius);
                Quaternion rotation = obj.transform.rotation;

                RaycastHit info;
                bool isInvalidPosition = false;
                if (Physics.Raycast(new Ray(position, Vector3.down), out info))
                {
                    if (info.collider.gameObject.tag != Plugin.TerrainTag && info.collider.gameObject.name != Plugin.TerrainName) // Did it hit terrain mesh? if not then position is not valid...
                    {
                        isInvalidPosition = true;
                    }
                }
                else // If didn't hit anything, position is is invalid
                {
                    isInvalidPosition = true;
                }
                foreach (Vector3 spawnDenialPoint in spawnDenialPoints)
                {
                    if (Vector3.Distance(position, spawnDenialPoint) <= 10.0f)
                    {
                        isInvalidPosition = true;
                    }
                }

                if (!isInvalidPosition)
                {
                    position.y = info.point.y; // Match raycast hit y position

                    position += offset;
                    rotation.eulerAngles = rotation.eulerAngles + new Vector3(0.0f, random.Next(0, 360), 0.0f);

                    GameObject gameObject = UnityEngine.Object.Instantiate(obj, position, rotation);

                    NetworkObject netObject = gameObject.GetComponent<NetworkObject>();
                    if (netObject != null) gameObject.GetComponent<NetworkObject>().Spawn(true);

                    Plugin.objectsToClear.Add(gameObject);
                }
            }
        }

        public static void SampleMap()
        {
            // Compute Map Area
            List<Vector2> OuterPoints = new List<Vector2>();
            foreach (GameObject outsideAiNode in RoundManager.Instance.outsideAINodes)
            {
                OuterPoints.Add(new Vector2(outsideAiNode.transform.position.x, outsideAiNode.transform.position.z));
            }
            OuterPoints = Functions.ComputeConvexHull(OuterPoints).ToList();

            float Area = 0.0f;
            for (int i = 0; i != OuterPoints.Count - 1; i++)
            {
                Vector2 from = OuterPoints[i], to = OuterPoints[i + 1];

                float averageHeight = (from.y + to.y) * 0.5f;
                float width = from.x - to.x;

                Area += averageHeight * width;
            }
            if (Area < 0.0f) Area *= -1.0f;
            Plugin.TerrainArea = Area;

            // Get terrainTag and terrainName
            List<Vector3> nodes = Functions.GetOutsideNodes();
            List<RaycastHit> hits = new List<RaycastHit>();
            for (int i = 0; i != nodes.Count * 10; i++) // 10 samples per node
            {
                Vector3 node = nodes[i % nodes.Count];
                Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * 3.0f;
                RaycastHit hit;
                if (Physics.Raycast(new Ray(node + new Vector3(randomPoint.x, 10.0f, randomPoint.y), Vector3.down), out hit))
                {
                    hits.Add(hit);
                }
            }

            Plugin.TerrainTag = Functions.MostCommon(hits.Select(x => x.collider.gameObject.tag).ToList());
            Plugin.TerrainName = Functions.MostCommon(hits.Select(x => x.collider.gameObject.name).ToList());
        }

        public static void DeliverRandomItems(int Amount, int MaxPrice)
        {
            if(RoundManager.Instance.IsServer)
            {
                Terminal terminal = GameObject.FindObjectOfType<Terminal>();

                List<int> validItems = new List<int>();
                for(int i = 0; i < terminal.buyableItemsList.Length; i++)
                {
                    if (terminal.buyableItemsList[i].creditsWorth <= MaxPrice) validItems.Add(i);
                }

                for(int i = 0; i < Amount; i++) {
                    int item = validItems[UnityEngine.Random.Range(0, validItems.Count)];
                    terminal.orderedItemsFromTerminal.Add(item);
                }
            }
        }

        public static Item GetScrapSafe(Item item)
        {
            if (Lists.ItemList.Any(x => x.Value.name == item.name))
            {
                foreach(KeyValuePair<Lists.ItemName, Item> i in Lists.ItemList)
                {
                    if (i.Value.name == item.name) return i.Value;
                }
            }
            if(Lists.CustomItemList.Any(x => x.Value.name == item.name))
            {
                foreach(KeyValuePair<string, Item> i in Lists.CustomItemList)
                {
                    if (i.Value.name == item.name) return i.Value;
                }
            }
            Log.LogError(string.Format("Item:'{0}' dosen't exist in the lists, returning metalSheet"));
            return Lists.ItemList[Lists.ItemName.MetalSheet];
        }

        public static void SpawnScrapOutside(int Amount)
        {
            Plugin.randomItemsToSpawnOutsideCount = Amount;
        }

        public static void DoSpawnScrapOutside(float Multiplier, int Amount = -1)
        {
            RoundManager r = RoundManager.Instance;
            System.Random rng = new System.Random();
            // Generate Scrap To Spawn
            int ScrapAmount = (int)(UnityEngine.Random.Range(r.currentLevel.minScrap, r.currentLevel.maxScrap + 1) * r.scrapAmountMultiplier * Multiplier);
            if (Amount != -1) ScrapAmount = (int)(Amount * r.scrapAmountMultiplier * Multiplier);
            List<Item> ScrapToSpawn = new List<Item>();
            List<int> ScrapValues = new List<int>();
            List<int> ScrapWeights = new List<int>();
            for(int i = 0; i < r.currentLevel.spawnableScrap.Count; i++)
            {
                if(i == r.increasedScrapSpawnRateIndex)
                {
                    ScrapWeights.Add(i);
                } 
                else
                {
                    ScrapWeights.Add(r.currentLevel.spawnableScrap[i].rarity);
                }
            }
            int[] weights = ScrapWeights.ToArray();
            for (int i = 0; i < ScrapAmount; i++)
            {
                Item pickedScrap = r.currentLevel.spawnableScrap[r.GetRandomWeightedIndex(weights, rng)].spawnableItem;
                ScrapToSpawn.Add(GetScrapSafe(pickedScrap)); // Get scrap safely
            }
            // Spawn Scrap
            List<NetworkObjectReference> ScrapSpawnsNet = new List<NetworkObjectReference>();
            List<Vector3> OutsideNodes = Functions.GetOutsideNodes();
            for(int i = 0; i < ScrapAmount; i++)
            {
                if (ScrapToSpawn[i] == null)
                {
                    Log.LogError("Found null element in list ScrapToSpawn. Skipping it.");
                    continue;
                }
                Vector3 position = r.GetRandomNavMeshPositionInBoxPredictable(OutsideNodes[UnityEngine.Random.Range(0, OutsideNodes.Count)], 10.0f, r.navHit, rng);
                GameObject obj = UnityEngine.Object.Instantiate(ScrapToSpawn[i].spawnPrefab, position, Quaternion.identity, r.spawnedScrapContainer);
                GrabbableObject grabbableObject = obj.GetComponent<GrabbableObject>();
                grabbableObject.transform.rotation = Quaternion.Euler(grabbableObject.itemProperties.restingRotation);
                grabbableObject.fallTime = 0.0f;
                ScrapValues.Add((int)(UnityEngine.Random.Range(ScrapToSpawn[i].minValue, ScrapToSpawn[i].maxValue + 1) * r.scrapValueMultiplier));
                grabbableObject.scrapValue = ScrapValues[ScrapValues.Count - 1];
                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                netObj.Spawn();
                ScrapSpawnsNet.Add(netObj);
            }
            r.StartCoroutine(waitForScrapToSpawnToSync(ScrapSpawnsNet.ToArray(), ScrapValues.ToArray()));
        }

        private static IEnumerator waitForScrapToSpawnToSync(NetworkObjectReference[] spawnedScrap, int[] scrapValues)
        {
            yield return new WaitForSeconds(11.0f);
            RoundManager.Instance.SyncScrapValuesClientRpc(spawnedScrap, scrapValues);
        }

        public static SpawnableItemWithRarity generateItemWithRarity(Item item, int rarity)
        {
            
            SpawnableItemWithRarity spawnableItemWithRarity = new SpawnableItemWithRarity();
            spawnableItemWithRarity.spawnableItem = item;
            spawnableItemWithRarity.rarity = rarity;
            if(item.spawnPrefab == null)
            {
                Log.LogError("Item prefab on generateItemWithRarity() is null, setting rarity to 0");
                spawnableItemWithRarity.rarity = 0;
            }
            return spawnableItemWithRarity;
        }

        public static void AddEnemyToPoolWithRarity(ref List<SpawnableEnemyWithRarity> list, EnemyType enemy, int rarity)
        {
            if(enemy.enemyPrefab == null)
            {
                Log.LogError("Enemy prefab is null on AddEnemyToPoolWithRarity(), returning.");
                return;
            }
            SpawnableEnemyWithRarity spawnableEnemyWithRarity = new SpawnableEnemyWithRarity();
            spawnableEnemyWithRarity.enemyType = enemy;
            spawnableEnemyWithRarity.rarity = rarity;
            list.Add(spawnableEnemyWithRarity);
        }

        public static void RemoveEnemySpawn(string Name)
        {
            bool removedEnemy = false;
            int index = RoundManager.Instance.currentLevel.Enemies.FindIndex(x => x.enemyType.name.ToUpper() == Name.ToUpper());
            if (index != -1)
            {
                RoundManager.Instance.currentLevel.Enemies.RemoveAt(index);
                removedEnemy = true;
            }
            index = RoundManager.Instance.currentLevel.OutsideEnemies.FindIndex(x => x.enemyType.name.ToUpper() == Name.ToUpper());
            if (index != -1)
            {
                RoundManager.Instance.currentLevel.OutsideEnemies.RemoveAt(index);
                removedEnemy = true;
            }
            index = RoundManager.Instance.currentLevel.DaytimeEnemies.FindIndex(x => x.enemyType.name.ToUpper() == Name.ToUpper());
            if (index != -1)
            {
                RoundManager.Instance.currentLevel.DaytimeEnemies.RemoveAt(index);
                removedEnemy = true;
            }
            if (!removedEnemy) Log.LogInfo(string.Format("Failed to remove '{0}' from enemy pool, either it dosen't exist on the map or wrong string used.", Name));
        }

        public static void SpawnOutsideEnemies(EnemyType enemy, int count)
        {
            Plugin.enemiesToSpawnOutside.Add(new cObj<EnemyType>(count, enemy));
        }

        public static void SpawnInsideEnemies(EnemyType enemy, int count)
        {
            Plugin.enemiesToSpawnInside.Add(new cObj<EnemyType>(count, enemy));
        }

        public static void DoSpawnOutsideEnemies()
        {
            List<Vector3> OutsideAiNodes = Functions.GetOutsideNodes();
            List<Vector3> SpawnDenialNodes = Functions.GetSpawnDenialNodes();

            // Spawn Outside enemies
            for (int i = 0; i < Plugin.enemiesToSpawnOutside.Count; i++)
            {
                for(int j = 0; j < Plugin.enemiesToSpawnOutside[i].count; j++)
                {
                    if (Plugin.enemiesToSpawnOutside[i]._object.enemyPrefab == null)
                    {
                        Log.LogError("Enemy prefab on DoSpawnOutsideEnemies() is null, continuing.");
                        continue;
                    }
                    GameObject obj = UnityEngine.Object.Instantiate(
                        Plugin.enemiesToSpawnOutside[i]._object.enemyPrefab,
                        Functions.GetSafePosition(OutsideAiNodes, SpawnDenialNodes, 20.0f),
                        Quaternion.Euler(Vector3.zero));

                    RoundManager.Instance.SpawnedEnemies.Add(obj.GetComponent<EnemyAI>());

                    obj.gameObject.GetComponentInChildren<NetworkObject>().Spawn(destroyWithScene: true);
                }
            }
            Plugin.enemiesToSpawnOutside.Clear();
        }

        public static void DoSpawnInsideEnemies()
        {
            // Spawn Inside enemies
            for (int i = 0; i < Plugin.enemiesToSpawnInside.Count; i++)
            {
                for (int j = 0; j < Plugin.enemiesToSpawnInside[i].count; j++)
                {
                    if (Plugin.enemiesToSpawnInside[i]._object.enemyPrefab == null)
                    {
                        Log.LogError("Enemy prefab on DoSpawnInsideEnemies() is null, continuing.");
                        continue;
                    }
                    int index = UnityEngine.Random.Range(0, RoundManager.Instance.allEnemyVents.Length);
                    Vector3 position = RoundManager.Instance.allEnemyVents[index].floorNode.position;
                    position = RoundManager.Instance.GetRandomNavMeshPositionInRadius(position, Plugin.enemiesToSpawnInside[i].radius, RoundManager.Instance.navHit);
                    Quaternion rotation = Quaternion.Euler(0.0f, RoundManager.Instance.allEnemyVents[index].floorNode.eulerAngles.y, 0.0f);
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Plugin.enemiesToSpawnInside[i]._object.enemyPrefab, position, rotation);

                    gameObject.GetComponentInChildren<NetworkObject>().Spawn(true);
                    EnemyAI component = gameObject.GetComponent<EnemyAI>();
                    RoundManager.Instance.SpawnedEnemies.Add(component);
                }
            }
            Plugin.enemiesToSpawnInside.Clear();
        }

        public static Event RandomWeightedEvent(List<Event> events)
        {
            int WeightedSum = 0;
            foreach (Event e in events) WeightedSum += e.Weight;

            foreach (Event e in events)
            {
                if (UnityEngine.Random.Range(0, WeightedSum) < e.Weight)
                {
                    return e;
                }
                WeightedSum -= e.Weight;
            }

            return events[events.Count - 1];
        }

        public static List<Event> ChooseEvents(SelectableLevel newLevel, List<Event> events, out List<Event> additionalEvents)
        {
            List<Event> chosenEvents = new List<Event>();
            List<Event> eventsToChooseForm = new List<Event>();
            foreach (Event e in events) eventsToChooseForm.Add(e);

            for(int i = 0; i < 3; i++)
            {
                Event newEvent = RandomWeightedEvent(eventsToChooseForm);
                chosenEvents.Add(newEvent);

                // Remove so no further accurrences
                eventsToChooseForm.RemoveAll(x => x.Name() == newEvent.Name());

                // Remove incompatible events from toChooseList
                int AmountRemoved = 0;

                foreach(string eventToRemove in newEvent.EventsToRemove())
                {
                    eventsToChooseForm.RemoveAll(x => x.Name() == eventToRemove);
                    AmountRemoved += chosenEvents.RemoveAll(x => x.Name() == eventToRemove);
                }

                foreach(string eventToSpawnWith in newEvent.EventsToSpawnWith())
                {
                    eventsToChooseForm.RemoveAll(x => x.Name() == eventToSpawnWith);
                    AmountRemoved += chosenEvents.RemoveAll(x => x.Name() == eventToSpawnWith);
                }

                i -= AmountRemoved; // Decrement each time an event is removed from chosenEvents list
            }

            // Generate eventsToSpawnWith list with no copies
            List<Event> eventsToSpawnWith = new List<Event>();
            for(int i = 0; i < chosenEvents.Count; i++)
            {
                foreach (string eventToSpawnWith in chosenEvents[i].EventsToSpawnWith())
                {
                    int index = eventsToSpawnWith.FindIndex(x => x.Name() == eventToSpawnWith);
                    if (index == -1) eventsToSpawnWith.Add(Event.GetEvent(eventToSpawnWith)); // If dosen't exist in list, add.
                }
            }

            // Remove incompatible events
            for(int i = 0; i < eventsToSpawnWith.Count; i++)
            {
                foreach(string eventToRemove in eventsToSpawnWith[i].EventsToRemove())
                {
                    eventsToSpawnWith.RemoveAll(x => x.Name() == eventToRemove);
                }
            }

            additionalEvents = new List<Event>();
            additionalEvents.AddRange(eventsToSpawnWith);
            return chosenEvents;
        }

        public static void ApplyEvents(List<Event> currentEvents)
        {
            foreach (Event e in currentEvents) e.Patch();
        }

        public static void UpdateAllEventWeights()
        {
            float eventTypeWeightSum = veryGoodWeight.Value + goodWeight.Value + neutralWeight.Value + badWeight.Value + veryBadWeight.Value + removeEnemyWeight.Value;

            float veryGoodProbability = veryGoodWeight.Value / eventTypeWeightSum;
            float goodProbablity = goodWeight.Value / eventTypeWeightSum;
            float neutralProbability = neutralWeight.Value / eventTypeWeightSum;
            float removeEnemyProbability = removeEnemyWeight.Value / eventTypeWeightSum;
            float badProbability = badWeight.Value / eventTypeWeightSum;
            float veryBadProbability = veryBadWeight.Value / eventTypeWeightSum;


            // Update all weights on events
            // CurrentSplit: VeryGood = 5%, Good = 18%, Neutral = 12%, RemoveEnemy = 15%, Bad = 35%, VeryBad = 15%
            int VeryGoodCount = 0, GoodCount = 0, NeutralCount = 0, RemoveEnemyCount = 0, BadCount = 0, VeryBadCount = 0, Sum = 0;
            foreach (Event e in Plugin.events)
            {
                switch (e.Type)
                {
                    case Event.type.VeryGood:
                        VeryGoodCount++;
                        break;
                    case Event.type.Good:
                        GoodCount++;
                        break;
                    case Event.type.Neutral:
                        NeutralCount++;
                        break;
                    case Event.type.RemoveEnemy:
                        RemoveEnemyCount++;
                        break;
                    case Event.type.Bad:
                        BadCount++;
                        break;
                    case Event.type.VeryBad:
                        VeryBadCount++;
                        break;
                }
            }

            Sum = VeryBadCount + GoodCount + NeutralCount + RemoveEnemyCount + BadCount + VeryBadCount;

            float VeryGoodWeight = (Sum / VeryGoodCount) * veryGoodProbability, GoodWeight = (Sum / GoodCount) * goodProbablity, NeutralWeight = (Sum / NeutralCount) * neutralProbability,
                  RemoveEnemyWeight = (Sum / RemoveEnemyCount) * removeEnemyProbability, BadWeight = (Sum / BadCount) * badProbability, VeryBadWeight = (Sum / VeryBadCount) * veryBadProbability;

            foreach (Event e in Plugin.events)
            {
                switch (e.Type)
                {
                    case Event.type.VeryGood:
                        e.Weight = (int)(VeryGoodWeight * 1000f);
                        break;
                    case Event.type.Good:
                        e.Weight = (int)(GoodWeight * 1000f);
                        break;
                    case Event.type.Neutral:
                        e.Weight = (int)(NeutralWeight * 1000f);
                        break;
                    case Event.type.RemoveEnemy:
                        e.Weight = (int)(RemoveEnemyWeight * 1000f);
                        break;
                    case Event.type.Bad:
                        e.Weight = (int)(BadWeight * 1000f);
                        break;
                    case Event.type.VeryBad:
                        e.Weight = (int)(VeryBadWeight * 1000f);
                        break;  
                }
                switch(e.Name())
                {
                }
            }
        }
    }
}
