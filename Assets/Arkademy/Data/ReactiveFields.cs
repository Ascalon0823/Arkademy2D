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

            [JsonIgnore] [SerializeField] protected long value;

            public long Value
            {
                get => value;
                set
                {
                    var oldValue = this.value;
                    this.value = value;
                    _onValueChange?.Invoke(oldValue, this.value);
                }
            }

            [JsonIgnore] private Action<long, long> _onValueChange;

            public class Handle : IDisposable
            {
                private readonly Field _field;
                private Action<long, long> _fieldValueChangeAction;
                public Handle(Field f, Action<long, long> action)
                {
                    _field = f;
                    _fieldValueChangeAction = action;
                    _field._onValueChange += action;
                }

                public void ForceTrigger()
                {
                    _field.Value = _field.Value;
                }
                public void Dispose()
                {
                    _field._onValueChange -= _fieldValueChangeAction;
                }
            }

            public Handle Subscribe(Action<long, long> valueChange, bool doOnSubscribe = true)
            {
                var handle = new Handle(this, valueChange);
                if(doOnSubscribe)_onValueChange?.Invoke(value, value);
                return handle;
            }
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
        [JsonIgnore] [SerializeField] protected List<Field> fields = new();

        protected void BuildCache()
        {
            foreach (var f in fields)
            {
                _valueCache[f.key] = f;
            }
        }
        public virtual List<Field> Fields
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
            if (!_valueCache.Any())
            {
                BuildCache();
            }
            return _valueCache.TryGetValue(key, out field);
        }

        public virtual void UpdateFieldsBy(ReactiveFields other)
        {
            if (!_valueCache.Any())
            {
               BuildCache();
            }
            foreach (var f in other.Fields)
            {
                if (!_valueCache.ContainsKey(f.key))
                {
                    var c = f.Copy();
                    Fields.Add(c);
                    _valueCache[f.key] = c;
                }
            }
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