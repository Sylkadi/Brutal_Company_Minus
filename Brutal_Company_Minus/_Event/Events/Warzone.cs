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
    internal class Warzone : Event
    {
        public override string Name() => nameof(Warzone);

        public override List<string> EventsToRemove() => new List<string>() { nameof(LeaflessBrownTrees), nameof(LeaflessTrees) };

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(Turrets), nameof(Landmines), nameof(OutsideTurrets), nameof(OutsideLandmines), nameof(Trees) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Landmines? Turrets? all of it";
            ColorHex = "#800000";
            Type = type.VeryBad;
        }
    }
}
