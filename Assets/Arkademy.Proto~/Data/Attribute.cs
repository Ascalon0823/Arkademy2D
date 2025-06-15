using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Arkademy.Data
{
    [Serializable]
    public partial class Attribute
    {
        public enum CalculationType
        {
            Flat,
            Percent,
            Chance
        }

        public string name;
        public Type type;
        public CalculationType calType;
        public long value;

        public Attribute Copy()
        {
            return new Attribute
            {
                type = type,
                calType = calType,
                value = value,
                current = current
            };
        }

        private long CalculateFlat(long baseValue)
        {
            if (Modifiers.TryGetValue(Modifier.Category.Replace, out var list))
            {
                baseValue = list.Max(x => x.value);
            }

            if (Modifiers.TryGetValue(Modifier.Category.Addition, out list))
            {
                foreach (var mod in list)
                {
                    baseValue += mod.value;
                }
            }

            if (Modifiers.TryGetValue(Modifier.Category.Multiplication, out list))
            {
                foreach (var mod in list)
                {
                    baseValue *= mod.value;
                    baseValue /= 10000;
                }
            }

            return baseValue;
        }

        private long CalculateChance(long baseValue)
        {
            if (Modifiers.TryGetValue(Modifier.Category.Replace, out var list))
            {
                baseValue = list.Max(x => x.value);
            }

            baseValue = 10000 - baseValue;
            if (Modifiers.TryGetValue(Modifier.Category.Addition, out list))
            {
                foreach (var mod in list)
                {
                    baseValue *= 10000 - mod.value;
                    baseValue /= 10000;
                }
            }

            if (Modifiers.TryGetValue(Modifier.Category.Multiplication, out list))
            {
                foreach (var mod in list)
                {
                    baseValue *= 10000 - mod.value;
                    baseValue /= 10000;
                }
            }

            return 10000 - baseValue;
        }


        public float Value(bool original = false)
        {
            return ToRealValue(BaseValue(original));
        }


        private float ToRealValue(long v)
        {
            switch (calType)
            {
                case CalculationType.Flat:
                    return v / 100f;
                case CalculationType.Percent:
                case CalculationType.Chance:
                    return v / 10000f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public long BaseValue(bool original = false)
        {
            if (original) return value;
            var final = value;
            switch (calType)
            {
                case CalculationType.Flat:
                case CalculationType.Percent:
                    final = CalculateFlat(final);
                    break;
                case CalculationType.Chance:
                    final = CalculateChance(final);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return final;
        }
    }
}