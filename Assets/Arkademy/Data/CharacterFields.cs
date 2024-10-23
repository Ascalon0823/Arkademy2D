using System;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    public partial class Character
    {
        [Serializable]
        public class Progression : ReactiveFields
        {
            public new Progression Copy()
            {
                return new Progression
                {
                    Fields = Fields.Select(x => x.Copy()).ToList()
                };
            }
        }


        [Serializable]
        public class Resources : ReactiveFields
        {
            [SerializeField] private ReactiveFields maximum = new();

            public ReactiveFields Max
            {
                get => maximum;
                set => maximum = value;
            }

            public override bool TryGet(string key, out Field field)
            {
                field = default;
                if (!maximum.TryGet(key, out var origin)) return false;
                if (!base.TryGet(key, out field))
                {
                    var f = origin.Copy();
                    f.Value = origin.Value;
                    Fields.Add(f);
                    _valueCache[f.key] = f;
                }

                return _valueCache.TryGetValue(key, out field);
            }

            public new Resources Copy()
            {
                return new Resources
                {
                    Max = Max.Copy()
                };
            }
        }

        [Serializable]
        public class Growth : ReactiveFields
        {
            [SerializeField] private ReactiveFields origin = new();

            public ReactiveFields Origin
            {
                get => origin;
                set => origin = value;
            }


            public override bool TryGet(string key, out Field field)
            {
                field = default;
                if (!Origin.TryGet(key, out var origin)) return false;
                if (!base.TryGet(key, out field))
                {
                    var f = origin.Copy();
                    f.Value = 0;
                    Fields.Add(f);
                    _valueCache[f.key] = f;
                }

                return _valueCache.TryGetValue(key, out field);
            }

            public new Growth Copy()
            {
                return new Growth
                {
                    Origin = Origin.Copy()
                };
            }
        }
    }
}