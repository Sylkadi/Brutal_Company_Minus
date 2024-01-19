using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class SmallerMap : Event
    {
        public override string Name() => nameof(SmallerMap);

        public override void Initalize()
        {
            Weight = 2;
            Description = "This facility is smaller.";
            ColorHex = "#008000";
            Type = type.Good;

            ScaleList.Add(ScaleType.FactorySize, new Scale(0.75f, 0.0f));
        }

        public override void Patch()
        {
            Plugin.factorySizeMultiplier *= Getf(ScaleType.FactorySize);
        }

    }
}
