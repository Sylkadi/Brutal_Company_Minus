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
    internal class LeaflessTrees : Event
    {
        public override string Name() => nameof(LeaflessTrees);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Trees), nameof(LeaflessBrownTrees) };

        public override void Initalize()
        {
            Weight = 8;
            Description = "These trees look dead";
            ColorHex = "#FFFFFF";
            Type = type.Neutral;

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.045f, 0.0f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.060f, 0.0f));
        }

        public override void Patch()
        {
            Server.Instance.outsideObjectsToSpawn.Add(new OutsideObjectsToSpawn(UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity)), (int)Lists.ObjectName.TreeLeafless2));
            Server.Instance.outsideObjectsToSpawn.Add(new OutsideObjectsToSpawn(UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity)), (int)Lists.ObjectName.TreeLeafless3));
        }

    }
}
