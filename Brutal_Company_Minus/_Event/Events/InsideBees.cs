using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class InsideBees : Event
    {
        public override string Name() => nameof(InsideBees);

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(Bees) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "BEES!! wait...";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(2.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(3.0f, 0.08f));
        }

        public override void Patch() => Plugin.enemiesToSpawnInside.Add(new cObj<EnemyType>(UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1), 10.0f, Lists.EnemyList[Lists.EnemyName.CircuitBee]));
    }
}
