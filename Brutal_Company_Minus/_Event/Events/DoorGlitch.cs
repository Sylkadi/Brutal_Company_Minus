using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class DoorGlitch : Event
    {
        public override string Name() => nameof(DoorGlitch);

        public override void Initalize()
        {
            Weight = 3;
            Description = "There is a ghost in the facility";
            ColorHex = "#FF0000";
            Type = type.Bad;
        }

        public override void Patch()
        {
            Plugin.DoorGlitchActive = true;
        }

    }
}
