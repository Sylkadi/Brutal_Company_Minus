using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoMasks : Event
    {
        public override string Name() => nameof(NoMasks);

        public override void Initalize()
        {
            Weight = 1;
            Description = "No masks";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch()
        {
            // Remove masked scrap
            int index = RoundManager.Instance.currentLevel.spawnableScrap.FindIndex(s => s.spawnableItem.name == Lists.ItemList[Lists.ItemName.Comedy].name);
            if (index != -1) RoundManager.Instance.currentLevel.spawnableScrap.RemoveAt(index);

            index = RoundManager.Instance.currentLevel.spawnableScrap.FindIndex(s => s.spawnableItem.name == Lists.ItemList[Lists.ItemName.Tragedy].name);
            if (index != -1) RoundManager.Instance.currentLevel.spawnableScrap.RemoveAt(index);

            // Remove masked enemy
            Manager.RemoveEnemySpawn("MaskedPlayerEnemy");
        }
    }
}
