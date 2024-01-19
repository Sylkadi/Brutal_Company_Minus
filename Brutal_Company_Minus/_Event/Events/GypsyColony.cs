using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class GypsyColony : Event
    {
        public override string Name() => nameof(GypsyColony);

        public override List<string> EventsToRemove() => new List<string>() { nameof(HoardingBugs) };

        public override List<string> EventsToSpawnWith() => new List<string> { nameof(ScarceOutsideScrap) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Theres too many of them";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(80.0f, 1.0f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(20.0f, 0.2f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(30.0f, 0.4f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(4.0f, 0.06f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(6.0f, 0.12f));
        }

        public override void Patch()
        {
            EnemyType HoardingBug = Lists.EnemyList[Lists.EnemyName.HoardingBug];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, HoardingBug, Get(ScaleType.EnemyRarity));

            Manager.SpawnOutsideEnemies(HoardingBug, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
            Manager.SpawnInsideEnemies(HoardingBug, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
