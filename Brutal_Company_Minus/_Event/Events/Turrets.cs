﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus._Event.Events
{
    internal class Turrets : Event
    {
        public override string Name() => nameof(Turrets);

        public override void Initalize()
        {
            Weight = 3;
            Description = "Turrets!!";
            ColorHex = "#FF0000";
            Type = type.Bad;


            ScaleList.Add(ScaleType.MinInsideEnemy, new Scale(15.0f, 0.5f));
            ScaleList.Add(ScaleType.MaxInsideEnemy, new Scale(25.0f, 0.75f));
        }

        public override void Patch()
        {
            AnimationCurve curve = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MaxInsideEnemy)), new Keyframe(1f, Get(ScaleType.MinInsideEnemy)));

            foreach(SpawnableMapObject obj in RoundManager.Instance.currentLevel.spawnableMapObjects)
            {
                if(obj.prefabToSpawn.name == Lists.ObjectList[Lists.ObjectName.Turret].name)
                {
                    obj.numberToSpawn = curve;
                }
            }
        }
    }
}
