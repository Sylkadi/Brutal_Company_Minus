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
    internal class OutsideLandmines : Event
    {
        public override string Name() => nameof(OutsideLandmines);

        public override void Initalize()
        {
            Weight = 3;
            Description = "There are landmines, Outside.";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.0030f, 0.00004f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.0045f, 0.000075f));
        }

        public override void Patch()
        {
            Plugin.insideObjectsToSpawnOutside.Add(new cObj<GameObject>(UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity)), Lists.ObjectList[Lists.ObjectName.Landmine]));
        }
    }
}
