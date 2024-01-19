using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class ChineseProduce : Event
    {
        public override string Name() => nameof(ChineseProduce);

        public override void Initalize()
        {
            Weight = 1;
            Description = "Everything here is made cheaply...";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.ScrapValue, new Scale(0.6f, 0.0f));
            ScaleList.Add(ScaleType.ScrapAmount, new Scale(2.0f, 0.0f));
        }

        public override void Patch()
        {
            Plugin.scrapValueMultiplier *= Getf(ScaleType.ScrapValue);
            Plugin.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
        }
    }
}
