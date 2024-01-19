using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoJester : Event
    {
        public override string Name() => nameof(NoJester);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Jester) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "It wont be there to fuck you up";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("Jester");
    }
}
