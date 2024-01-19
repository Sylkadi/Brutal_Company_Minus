using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Thumpers : Event
    {
        public override string Name() => nameof(Thumpers);

        public override void Initalize()
        {
            Weight = 3;
            Description = "You need to run";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(40.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(1.0f, 0.07f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(1.0f, 0.1f));
        }

        public override void Patch()
        {
            EnemyType Thumper = Lists.EnemyList[Lists.EnemyName.Thumper];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Thumper, Get(ScaleType.EnemyRarity));
            Manager.SpawnInsideEnemies(Thumper, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
