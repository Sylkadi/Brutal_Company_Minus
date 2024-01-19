using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class NoLandmines : Event
    {
        public override string Name() => nameof(NoLandmines);

        public override List<string> EventsToRemove() => new List<string>() { nameof(Landmines), nameof(OutsideLandmines), nameof(Warzone) };

        public override void Initalize()
        {
            Weight = 1;
            Description = "No landmines";
            ColorHex = "#008000";
            Type = type.RemoveEnemy;
        }

        public override void Patch()
        {
            AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f));

            foreach (SpawnableMapObject obj in RoundManager.Instance.currentLevel.spawnableMapObjects)
            {
                if (obj.prefabToSpawn.name == Lists.ObjectList[Lists.ObjectName.Landmine].name)
                {
                    obj.numberToSpawn = curve;
                }
            }
        }

    }
}
