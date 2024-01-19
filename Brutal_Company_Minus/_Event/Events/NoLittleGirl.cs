using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoLittleGirl : Event
    {
        public override string Name() => nameof(NoLittleGirl);

        public override List<string> EventsToRemove() => new List<string>() { nameof(LittleGirl) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No little shits";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("DressGirl");
    }
}
