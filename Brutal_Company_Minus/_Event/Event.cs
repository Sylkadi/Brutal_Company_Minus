using Brutal_Company_Minus;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Brutal_Company_Minus._Event
{
    public class Event
    {
        public string Description = "";
        public string ColorHex = "#FFFFFF";
        public int Weight = 1;
        public type Type = type.Default;
        public bool Enabled = true;

        public Dictionary<ScaleType, Scale> ScaleList = new Dictionary<ScaleType, Scale>();

        public virtual string Name() => "";
        public virtual List<string> EventsToRemove() => new List<string>();

        public virtual List<string> EventsToSpawnWith() => new List<string>();

        public virtual void Initalize() { }

        public virtual void Patch() { }

        public enum type
        {
            Default, VeryBad, Bad, Neutral, Good, VeryGood, RemoveEnemy
        }

        public int Get(ScaleType type)
        {
            Scale scale = new Scale(0.0f, 0.0f);
            try
            {
                scale = ScaleList[type];
            } catch
            {
                Log.LogError(string.Format("Scalar '{0}' for '{1}' not found, returning 0.", type.ToString(), Name()));
            }
            return (int)(scale.BaseScale + (scale.IncrementScale * Plugin.DaysPassed));
        }

        public float Getf(ScaleType type)
        {
            Scale scale = new Scale(0.0f, 0.0f);
            try
            {
                scale = ScaleList[type];
            } 
            catch
            {
                Log.LogError(string.Format("Scalar '{0}' for '{1}' not found, returning 0.0f.", type.ToString(), Name()));
            }
            return scale.BaseScale + (scale.IncrementScale * Plugin.DaysPassed);
        }

        public static Event GetEvent(string name)
        {
            int index = Plugin.events.FindIndex(x => x.Name() == name);
            if(index != -1) return Plugin.events[index];

            Log.LogError(string.Format("Event '{0}' dosen't exist, returning random event", name));
            return Plugin.events[UnityEngine.Random.Range(0, Plugin.events.Count)];
        }
    }
}
