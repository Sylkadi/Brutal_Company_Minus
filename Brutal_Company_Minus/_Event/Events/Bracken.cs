using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Bracken : Event
    {
        public override string Name() => nameof(Bracken);
        public override void Initalize()
        {
            Weight = 1;
            Description = "Your neck tingles";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(40.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(1.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(2.0f, 0.1f));
        }
        public override void Patch()
        {
            EnemyType Bracken = Lists.EnemyList[Lists.EnemyName.Bracken];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Bracken, Get(ScaleType.EnemyRarity));
            Manager.SpawnInsideEnemies(Bracken, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
