using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoCoilhead : Event
    {
        public override string Name() => nameof(NoCoilhead);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Coilhead), nameof(AntiCoilhead) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Nothing to stare at today";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("SpringMan");
    }
}
