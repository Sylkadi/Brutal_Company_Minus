using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoWorm : Event
    {
        public override string Name() => nameof(NoWorm);

        public override void Initalize()
        {
            Weight = 1;
            Description = "No worms";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("SandWorm");
    }
}
