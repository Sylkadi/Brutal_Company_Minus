using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class ScarceOutsideScrap : Event
    {
        public override string Name() => nameof(ScarceOutsideScrap);

        public override void Initalize()
        {
            Weight = 1;
            Description = "There is a scarce amount of scrap outside.";
            ColorHex = "#008000";
            Type = type.Good;

            ScaleList.Add(ScaleType.MinItemAmount, new Scale(3.0f, 0.07f));
            ScaleList.Add(ScaleType.MaxItemAmount, new Scale(5.0f, 0.1f));
        }

        public override void Patch() => Manager.SpawnScrapOutside(UnityEngine.Random.Range(Get(ScaleType.MinItemAmount), Get(ScaleType.MaxItemAmount) + 1));
    }
}
