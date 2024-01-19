using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Jester : Event
    {
        public override string Name() => nameof(Jester);

        public override void Initalize()
        {
            Weight = 1;
            Description = "I want to go home";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(2.0f, 0.03f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(3.0f, 0.05f));
        }

        public override void Patch()
        {
            Manager.SpawnInsideEnemies(Lists.EnemyList[Lists.EnemyName.Jester], UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
