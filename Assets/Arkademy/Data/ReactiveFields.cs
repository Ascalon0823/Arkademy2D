using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class ReactiveFields
    {
        [Serializable]
        public class Field
        {
            public string key;

            [JsonIgnore][SerializeField]protected long value;

            public long Value
            {
                get => value;
                set
                {
                    var oldValue = this.value;
                    this.value = value;
                    OnValueChange?.Invoke(oldValue, this.value);
                }
            }

            [JsonIgnore] public Action<long, long> OnValueChange;

            public Field Copy()
            {
                return new Field
                {
                    key = key,
                    value = value,
                };
            }
        }

        [JsonIgnore] protected Dictionary<string, Field> _valueCache = new();
        [JsonIgnore] [SerializeField]protected List<Field> fields = new();

        public List<Field> Fields
        {
            get => fields;
            set
            {
                fields = value;
                _valueCache.Clear();
                foreach (var field in fields)
                {
                    _valueCache[field.key] = field;
                }
            }
        }
        public virtual bool TryGet(string key, out Field field)
        {
            return _valueCache.TryGetValue(key, out field);
        }
        public ReactiveFields Copy()
        {
            return new ReactiveFields
            {
                fields = fields.Select(x => x.Copy()).ToList()
            };
        }
    }
}