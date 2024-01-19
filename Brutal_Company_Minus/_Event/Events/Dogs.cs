using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Dogs : Event
    {
        public override string Name() => nameof(Dogs);

        public override void Initalize()
        {
            Weight = 1;
            Description = "They can hear you";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(20.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(1.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(1.0f, 0.1f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(0.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(1.0f, 0.08f));
        }

        public override void Patch()
        {
            EnemyType Dog = Lists.EnemyList[Lists.EnemyName.EyelessDog];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Dog, Get(ScaleType.EnemyRarity));

            Manager.SpawnOutsideEnemies(Dog, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
            Manager.SpawnInsideEnemies(Dog, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
