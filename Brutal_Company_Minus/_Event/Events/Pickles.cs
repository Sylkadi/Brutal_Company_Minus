using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Pickles : Event
    {
        public override string Name() => nameof(Pickles);

        public override List<string> EventsToRemove() => new List<string>() { nameof(TransmuteScrapBig), nameof(TransmuteScrapSmall), nameof(Dentures), nameof(GoldenFacility), nameof(Honk), nameof(GoldenBars) };

        public override void Initalize()
        {
            Weight = 2;
            Description = "Tastes salty...";
            ColorHex = "#008000";
            Type = type.Good;

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.08f, 0.0015f));
        }

        public override void Patch()
        {
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);

            RoundManager.Instance.currentLevel.spawnableScrap.Clear();
            RoundManager.Instance.currentLevel.spawnableScrap.Add(Manager.generateItemWithRarity(Lists.ItemList[Lists.ItemName.JarOfPickles], 100));
        }
    }
}
