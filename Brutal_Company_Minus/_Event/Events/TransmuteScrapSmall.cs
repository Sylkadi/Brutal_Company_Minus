using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class TransmuteScrapSmall : Event
    {
        public override string Name() => nameof(TransmuteScrapSmall);

        public override List<string> EventsToRemove() => new List<string>() { nameof(TransmuteScrapBig), nameof(Dentures), nameof(Pickles), nameof(GoldenFacility), nameof(Honk), nameof(GoldenBars) };

        public override void Initalize()
        {
            Weight = 2;
            Description = "All the scrap has transmuted into something small...";
            ColorHex = "#008000";
            Type = type.Good;

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.08f, 0.0015f));
        }

        public override void Patch()
        {
            // Remove all big scrap
            List<SpawnableItemWithRarity> BigScrapList = new List<SpawnableItemWithRarity>();
            foreach (SpawnableItemWithRarity item in RoundManager.Instance.currentLevel.spawnableScrap)
            {
                if (item.spawnableItem.twoHanded) BigScrapList.Add(item);
            }
            foreach (SpawnableItemWithRarity item in BigScrapList)
            {
                RoundManager.Instance.currentLevel.spawnableScrap.Remove(item);
            }

            SpawnableItemWithRarity chosenScrap = RoundManager.Instance.currentLevel.spawnableScrap[UnityEngine.Random.Range(0, RoundManager.Instance.currentLevel.spawnableScrap.Count)];

            RoundManager.Instance.currentLevel.spawnableScrap.RemoveAll(x => x.spawnableItem.name != chosenScrap.spawnableItem.name);

            foreach (SpawnableItemWithRarity item in RoundManager.Instance.currentLevel.spawnableScrap)
            {
                if (chosenScrap.spawnableItem.name == item.spawnableItem.name) item.rarity = 100;
            }

            // Scale scrap amount abit more
            Plugin.scrapMinAmount += RoundManager.Instance.currentLevel.minTotalScrapValue / chosenScrap.spawnableItem.minValue;
            Plugin.scrapMaxAmount += RoundManager.Instance.currentLevel.maxTotalScrapValue / chosenScrap.spawnableItem.maxValue;
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
        }
    }
}
