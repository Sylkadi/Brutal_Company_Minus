using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class GoldenFacility : Event
    {
        public override string Name() => nameof(GoldenFacility);

        public override List<string> EventsToRemove() => new List<string>() { nameof(TransmuteScrapBig), nameof(TransmuteScrapSmall), nameof(Dentures), nameof(Pickles), nameof(Honk), nameof(GoldenBars) };

        public override void Initalize()
        {
            Weight = 2;
            Description = "The scrap looks shiny";
            ColorHex = "#008000";
            Type = type.Good;

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.08f, 0.0015f));
        }

        public override void Patch()
        {
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);

            RoundManager.Instance.currentLevel.spawnableScrap.Clear();

            List<SpawnableItemWithRarity> ScrapToSpawn = new List<SpawnableItemWithRarity>
            {
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.GoldenCup], 25),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.Ring], 20),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.GoldBar], 2),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.FancyLamp], 10),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.PerfumeBottle], 25),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.Painting], 15),
                    Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.CashRegister], 3)
            };

            foreach (SpawnableItemWithRarity scrap in ScrapToSpawn) RoundManager.Instance.currentLevel.spawnableScrap.Add(scrap);
        }
    }
}
