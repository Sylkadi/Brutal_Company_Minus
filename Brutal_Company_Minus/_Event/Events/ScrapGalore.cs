using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class ScrapGalore : Event
    {
        public override string Name() => nameof(ScrapGalore);

        public override void Initalize()
        {
            Weight = 1;
            Description = "Scrap here is plentiful and of high quality.";
            ColorHex = "#00FF00";
            Type = type.VeryGood;

            ScaleList.Add(ScaleType.ScrapValue, new Scale(1.35f, 0.006f));
            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.35f, 0.006f));
        }

        public override void Patch()
        {
            Plugin.scrapValueMultiplier *= Getf(ScaleType.ScrapValue);
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
        }
    }
}
