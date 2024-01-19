using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoSlimes : Event
    {
        public override string Name() => nameof(NoSlimes);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Slimes) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No goo";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("Blob");
    }
}
