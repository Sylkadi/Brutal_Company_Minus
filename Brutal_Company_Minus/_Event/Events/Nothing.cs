using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Nothing : Event
    {
        public override string Name() => nameof(Nothing);

        public override void Initalize()
        {
            Weight = 8;
            Description = "--Nothing--";
            ColorHex = "#FFFFFF";
            Type = type.Neutral;
        }
    }
}
