using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class PlentyOutsideScrap : Event
    {
        public override string Name() => nameof(PlentyOutsideScrap);

        public override List<string> EventsToRemove() => new List<string>() { nameof(ScarceOutsideScrap) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "There is some scrap to be found outside.";
            ColorHex = "#00FF00";
            Type = type.VeryGood;

            ScaleList.Add(ScaleType.MinItemAmount, new Scale(8.0f, 0.1f));
            ScaleList.Add(ScaleType.MaxItemAmount, new Scale(12.0f, 0.15f));
        }

        public override void Patch() => Manager.SpawnScrapOutside(1.0f, UnityEngine.Random.Range(Get(ScaleType.MinItemAmount), Get(ScaleType.MaxItemAmount) + 1));
    }
}
