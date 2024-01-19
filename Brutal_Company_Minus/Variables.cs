using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace Brutal_Company_Minus
{
    public struct cObj<T>
    {
        public float count;
        public T _object;
        public float radius;

        public cObj(float Count, T Object)
        {
            count = Count;
            _object = Object;
            radius = 0.0f;
        }

        public cObj(float Count, float Radius, T Object)
        {
            count = Count;
            _object = Object; 
            radius = Radius;
        }
    }

    public struct OutsideObjectsToSpawn : INetworkSerializable, IEquatable<OutsideObjectsToSpawn>
    {
        public float density;
        public int objectEnumID;

        public OutsideObjectsToSpawn(float density, int objectEnumID)
        {
            this.density = density;
            this.objectEnumID = objectEnumID;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                FastBufferReader reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out density);
                reader.ReadValueSafe(out objectEnumID);
            }
            else
            {
                FastBufferWriter write = serializer.GetFastBufferWriter();
                write.WriteValueSafe(density);
                write.WriteValueSafe(objectEnumID);
            }
        }

        public bool Equals(OutsideObjectsToSpawn other)
        {
            return (objectEnumID == other.objectEnumID) && (density == other.density);
        }
    }

    public enum ScaleType
    {
        EnemyRarity, MinOutsideEnemy, MinInsideEnemy, MaxOutsideEnemy, MaxInsideEnemy, ScrapValue, ScrapAmount, FactorySize, MinDensity, MaxDensity, MinCash, MaxCash, MinItemAmount, MaxItemAmount, MinValue, MaxValue, Other
    }

    public struct Scale
    {
        public float BaseScale;
        public float IncrementScale;

        public Scale(float baseScale, float incrementScale)
        {
            BaseScale = baseScale;
            IncrementScale = incrementScale;
        }
    }
}
