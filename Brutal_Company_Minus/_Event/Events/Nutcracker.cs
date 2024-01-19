using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Nutcracker : Event
    {
        public override string Name() => nameof(Nutcracker);

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(Turrets) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Average american school experience";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(50.0f, 0.75f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(2.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(3.0f, 0.08f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(0.0f, 0.03f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(1.0f, 0.05f));
        }

        public override void Patch()
        {
            EnemyType Nutcracker = Lists.EnemyList[Lists.EnemyName.NutCracker];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, Nutcracker, Get(ScaleType.EnemyRarity));

            Manager.SpawnInsideEnemies(Nutcracker, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
            Manager.SpawnOutsideEnemies(Nutcracker, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
        }
    }
}
