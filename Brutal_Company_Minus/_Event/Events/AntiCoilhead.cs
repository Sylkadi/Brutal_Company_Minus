using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class AntiCoilhead : Event
    {
        public override string Name() => nameof(AntiCoilhead);

        public override void Initalize()
        {
            Weight = 1;
            Description = "Dont look at them...";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(30.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(1.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(1.0f, 0.08f));
        }

        public override void Patch()
        {
            EnemyType Coilhead = Lists.EnemyList[Lists.EnemyName.CoilHead];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Coilhead, Get(ScaleType.EnemyRarity));
            Manager.SpawnInsideEnemies(Coilhead, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));

            Server.Instance.isAntiCoilHead.Value = true;
        }
    }
}
