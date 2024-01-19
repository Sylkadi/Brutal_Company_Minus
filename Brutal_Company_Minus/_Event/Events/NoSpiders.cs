using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoSpiders : Event
    {
        public override string Name() => nameof(NoSpiders);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Spiders), nameof(Arachnophobia) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No 8 legged fucks";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch() => Manager.RemoveEnemySpawn("SandSpider");
    }
}
