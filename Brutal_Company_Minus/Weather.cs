using Brutal_Company_Minus;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Brutal_Company_Minus
{
    public struct Weather : INetworkSerializable, IEquatable<Weather>
    {

        public LevelWeatherType weatherType;

        public float scrapValueMultiplier;
        public float scrapAmountMultiplier;
        public float factorySizeMultiplier;

        public Weather(LevelWeatherType weatherType, float scrapValueMultiplier, float scrapAmountMultiplier, float factorySizeMultiplier)
        {
            this.weatherType = weatherType;
            this.scrapValueMultiplier = scrapValueMultiplier;
            this.scrapAmountMultiplier = scrapAmountMultiplier;
            this.factorySizeMultiplier = factorySizeMultiplier;
        }

        public static Weather operator *(Weather left, Weather right)
        {
            return new Weather(left.weatherType, left.scrapValueMultiplier * right.scrapValueMultiplier, left.scrapAmountMultiplier * right.scrapAmountMultiplier, left.factorySizeMultiplier * right.factorySizeMultiplier);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if(serializer.IsReader)
            {
                FastBufferReader reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out weatherType);
                reader.ReadValueSafe(out scrapValueMultiplier);
                reader.ReadValueSafe(out scrapAmountMultiplier);
                reader.ReadValueSafe(out factorySizeMultiplier);
            } else
            {
                FastBufferWriter writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(weatherType);
                writer.WriteValueSafe(scrapValueMultiplier);
                writer.WriteValueSafe(scrapAmountMultiplier);
                writer.WriteValueSafe(factorySizeMultiplier);
            }
        }

        public bool Equals(Weather other)
        {
            return weatherType == other.weatherType;
        }

        public static NetworkList<Weather> InitalizeWeatherMultipliers(NetworkList<Weather> currentWeatherMultipliers)
        {
            // Set Initial Values
            currentWeatherMultipliers.Add(Configuration.noneMultiplier);
            currentWeatherMultipliers.Add(Configuration.dustCloudMultiplier);
            currentWeatherMultipliers.Add(Configuration.rainyMultiplier);
            currentWeatherMultipliers.Add(Configuration.stormyMultiplier);
            currentWeatherMultipliers.Add(Configuration.foggyMultiplier);
            currentWeatherMultipliers.Add(Configuration.floodedMultiplier);
            currentWeatherMultipliers.Add(Configuration.eclipsedMultiplier);

            return currentWeatherMultipliers;
        }

        public static NetworkList<Weather> RandomizeWeatherMultipliers(NetworkList<Weather> currentWeatherMultipliers)
        {
            if(!Configuration.randomizeWeatherMultipliers.Value) return currentWeatherMultipliers;
            for(int i = 0; i < currentWeatherMultipliers.Count; i++)
            {
                float minInclusive = Configuration.weatherRandomRandomMinInclusive.Value, maxInclusive = Configuration.weatherRandomRandomMaxInclusive.Value;

                Weather multipliers = new Weather(currentWeatherMultipliers[i].weatherType, UnityEngine.Random.Range(minInclusive, maxInclusive), UnityEngine.Random.Range(minInclusive, maxInclusive), UnityEngine.Random.Range(minInclusive, maxInclusive));

                switch (currentWeatherMultipliers[i].weatherType)
                {
                    case LevelWeatherType.None:
                        currentWeatherMultipliers[i] = multipliers * Configuration.noneMultiplier;
                        break;
                    case LevelWeatherType.DustClouds:
                        currentWeatherMultipliers[i] = multipliers * Configuration.dustCloudMultiplier;
                        break;
                    case LevelWeatherType.Rainy:
                        currentWeatherMultipliers[i] = multipliers * Configuration.rainyMultiplier;
                        break;
                    case LevelWeatherType.Stormy:
                        currentWeatherMultipliers[i] = multipliers * Configuration.stormyMultiplier;
                        break;
                    case LevelWeatherType.Foggy:
                        currentWeatherMultipliers[i] = multipliers * Configuration.foggyMultiplier;
                        break;
                    case LevelWeatherType.Flooded:
                        currentWeatherMultipliers[i] = multipliers * Configuration.floodedMultiplier;
                        break;
                    case LevelWeatherType.Eclipsed:
                        currentWeatherMultipliers[i] = multipliers * Configuration.eclipsedMultiplier;
                        break;
                }
            }
            return currentWeatherMultipliers;
        }

    }
}
