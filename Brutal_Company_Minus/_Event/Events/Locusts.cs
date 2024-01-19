using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Locusts : Event
    {
        public override string Name() => nameof(Locusts);

        public override void Initalize()
        {
            Weight = 8;
            Description = "Locust season is here";
            ColorHex = "#FFFFFF";
            Type = type.Neutral;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(40.0f, 0.5f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(8.0f, 0.0f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(12.0f, 0.0f));
        }

        public override void Patch()
        {
            EnemyType Locust = Lists.EnemyList[Lists.EnemyName.RoamingLocust];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.DaytimeEnemies, Locust, Get(ScaleType.EnemyRarity));
            Manager.SpawnOutsideEnemies(Locust, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
        }
    }
}
