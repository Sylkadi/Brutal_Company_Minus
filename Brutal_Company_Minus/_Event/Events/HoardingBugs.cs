using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class HoardingBugs : Event
    {
        public override string Name() => nameof(HoardingBugs);

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(ScarceOutsideScrap) };

        public override void Initalize()
        {
            Weight = 3;
            Description = "They look cute.";
            ColorHex = "#FF0000";
            Type = type.Bad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(40.0f, 0.5f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(2.0f, 0.125f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(4.0f, 0.25f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(1.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(2.0f, 0.1f));
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
