using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Arachnophobia : Event
    {
        public override string Name() => nameof(Arachnophobia);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Trees), nameof(LeaflessBrownTrees) };

        public override List<string> EventsToSpawnWith() => new List<string>() { nameof(LeaflessTrees) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "Nightmare Facility";
            ColorHex = "#800000";
            Type = type.VeryBad;

            ScaleList.Add(ScaleType.EnemyRarity, new Scale(80.0f, 1.0f));
            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(7.0f, 0.1f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(11.0f, 0.2f));
            ScaleList.Add(ScaleType.MinOutsideEnemy, new Scale(3.0f, 0.05f));
            ScaleList.Add(ScaleType.MaxOutsideEnemy, new Scale(5.0f, 0.1f));
        }

        public override void Patch()
        {
            EnemyType BunkerSpider = Lists.EnemyList[Lists.EnemyName.BunkerSpider];

            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, BunkerSpider, Get(ScaleType.EnemyRarity));

            Manager.SpawnOutsideEnemies(BunkerSpider, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
            Manager.SpawnInsideEnemies(BunkerSpider, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));
        }
    }
}
