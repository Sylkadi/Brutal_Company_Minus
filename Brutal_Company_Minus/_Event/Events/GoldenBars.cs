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
    internal class GoldenBars : Event
    {
        public override string Name() => nameof(GoldenBars);

        public override List<string> EventsToRemove() => new List<string>() { nameof(TransmuteScrapBig), nameof(TransmuteScrapSmall), nameof(Dentures), nameof(Pickles), nameof(GoldenFacility), nameof(Honk) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Bling bling";
            ColorHex = "#00FF00";
            Type = type.VeryGood;

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.1f, 0.0025f));
        }

        public override void Patch()
        {
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);

            RoundManager.Instance.currentLevel.spawnableScrap.Clear();
            RoundManager.Instance.currentLevel.spawnableScrap.Add(Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.GoldBar], 100));
        }
    }
}
