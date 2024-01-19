using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Lizard : Event
    {
        public override string Name() => nameof(Lizard);

        public override void Initalize()
        {
            Weight = 3;
            Description = "They dont bite... i swear";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(35.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(1.0f, 0.06f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(2.0f, 0.12f));
        }

        public override void Patch()
        {
            EnemyType Lizard = Lists.EnemyList[Lists.EnemyName.SporeLizard];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Lizard, Get(ScaleType.EnemyRarity));
            Manager.SpawnInsideEnemies(Lizard, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
