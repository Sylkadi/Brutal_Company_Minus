using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class BigBonus : Event
    {
        public override string Name() => nameof(BigBonus);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Bonus) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Corporate is very pleased";
            ColorHex = "#00FF00";
            Type = type.VeryGood;

            ScaleList.Add(ScaleType.MinCash, new Scale(200.0f, 1.5f));
            ScaleList.Add(ScaleType.MaxCash, new Scale(300.0f, 3.0f));
        }

        public override void Patch()
        {
            Plugin.Paycut += UnityEngine.Random.Range(Get(ScaleType.MinCash), Get(ScaleType.MaxCash) + 1);
        }
    }
}
