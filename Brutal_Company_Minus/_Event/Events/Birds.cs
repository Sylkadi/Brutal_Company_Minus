using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Birds : Event
    {
        public override string Name() => nameof(Birds);

        public override void Initalize()
        {
            Weight = 8;
            Description = "Its bird migration season";
            ColorHex = "#FFFFFF";
            Type = type.Neutral;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(30.0f, 0.5f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(8.0f, 0.0f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(12.0f, 0.0f));
        }

        public override void Patch()
        {
            EnemyType Bird = Lists.EnemyList[Lists.EnemyName.Manticoil];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.DaytimeEnemies, Bird, Get(ScaleType.EnemyRarity));
            Manager.SpawnOutsideEnemies(Bird, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
        }
    }
}
