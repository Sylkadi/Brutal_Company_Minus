using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoThumpers : Event
    {
        public override string Name() => nameof(NoThumpers);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Thumpers) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No crawlers";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("Crawler");
    }
}
