using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class SnareFleas : Event
    {
        public override string Name() => nameof(SnareFleas);

        public override void Initalize()
        {
            Weight = 3;
            Description = "Ceiling campers!";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(35.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(2.0f, 0.1f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(3.0f, 0.15f));
        }

        public override void Patch()
        {
            EnemyType SnareFlea = Lists.EnemyList[Lists.EnemyName.SnareFlea];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, SnareFlea, Get(ScaleType.EnemyRarity));
            Manager.SpawnInsideEnemies(SnareFlea, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
