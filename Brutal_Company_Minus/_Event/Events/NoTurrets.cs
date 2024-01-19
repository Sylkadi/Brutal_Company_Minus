using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoTurrets : Event
    {
        public override string Name() => nameof(NoTurrets);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Turrets), nameof(OutsideTurrets), nameof(Warzone) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No turrets";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch()
        {
            AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f));

            foreach (SpawnableMapObject obj in RoundManager.Instance.currentLevel.spawnableMapObjects)
            {
                if (obj.prefabToSpawn.name == Lists.ObjectList[Lists.ObjectName.Turret].name)
                {
                    obj.numberToSpawn = curve;
                }
            }
        }

    }
}
