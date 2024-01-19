using Brutal_Company_Minus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class OutsideTurrets : Event
    {
        public override string Name() => nameof(OutsideTurrets);

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(Trees) };

        public override void Initalize()
        {
            Weight = 3;
            Description = "The turrets blend in with the trees...";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.0014f, 0.000023f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.0020f, 0.000030f));
        }

        public override void Patch()
        {
            Plugin.insideObjectsToSpawnOutside.Add(new cObj<GameObject>(UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity)), Lists.ObjectList[Lists.ObjectName.Turret]));
        }
    }
}
