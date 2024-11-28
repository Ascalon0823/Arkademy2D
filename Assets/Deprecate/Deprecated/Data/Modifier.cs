using System;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Modifier : Field
    {
        public enum Type
        {
            Flat,
            AdditivePercent,
            MutiplicativePercent,
            Chance,
        }

        public Type type;

        public Modifier(string key, long value, Type type) : base(key, value)
        {
            this.type = type;
        }

        public new Modifier Copy()
        {
            return new Modifier(this.key, this.Value, this.type);
        }
    }

    public static class Extensions
    {
        public static long Calculate(this Modifier.Type type, long[] values)
        {
            float ret;
            switch (type)
            {
                case Modifier.Type.Flat:
                    return values.Sum();
                case Modifier.Type.AdditivePercent:
                    return values.Sum();
                case Modifier.Type.Chance:
                    ret = 1f;
                    foreach (var value in values)
                    {
                        var v = Mathf.Clamp(value, 0, 10000);
                        v = 10000 - v;
                        ret *= v / 10000f;
                    }

                    return Mathf.FloorToInt((1 - ret) * 10000);
                case Modifier.Type.MutiplicativePercent:
                    ret = 1f;
                    foreach (var value in values)
                    {
                        ret *= (1 + value / 10000f);
                    }
                    return Mathf.FloorToInt((ret - 1) * 10000);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}