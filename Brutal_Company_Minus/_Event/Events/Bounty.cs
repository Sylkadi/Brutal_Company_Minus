using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Bounty : Event
    {
        public override string Name() => nameof(Bounty);

        public override void Initalize()
        {
            Weight = 2;
            Description = "The company is now paying for kills";
            ColorHex = "#008000";
            Type = type.Good;
        }

        public override void Patch()
        {
            Plugin.BountyActive = true;
        }

    }
}
