using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    public interface ISubscription : IDisposable
    {
        public void Trigger();
    }

    [Serializable]
    public class Field
    {
        public string key;
        [SerializeField] private long value;
        protected Action<long> OnValueChanged;

        public long Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(GetValue());
            }
        }

        public virtual long GetValue()
        {
            return value;
        }
        public Field(string key, long value)
        {
            this.key = key;
            this.value = value;
        }

        public virtual ISubscription Subscribe(Action<long> onValueChanged, bool doOnSubscribe = true)
        {
            return new Handle(this, onValueChanged, doOnSubscribe);
        }

        public virtual Field Copy()
        {
            return new Field(this.key, this.value);
        }

        public class Handles : ISubscription
        {
            private readonly Dictionary<Field, ISubscription> _handles = new();

            public Handles(List<Field> fields, Action<List<Field>> onUpdate,
                bool doOnSubscribe = true)
            {
                foreach (var f in fields)
                {
                    _handles[f] = f.Subscribe((_) => { onUpdate?.Invoke(_handles.Keys.ToList()); }, false);
                }

                if (doOnSubscribe) Trigger();
            }

            public void Dispose()
            {
                foreach (var handle in _handles.Values)
                {
                    handle.Dispose();
                }

                _handles.Clear();
            }

            public void Trigger()
            {
                _handles.Values.First().Trigger();
            }
        }

        public class Handle : ISubscription
        {
            private readonly Field _field;
            private readonly Action<long> _fieldValueChangeAction;

            public Handle(Field f, Action<long> action, bool doOnSubscribe = true)
            {
                _field = f;
                _fieldValueChangeAction = action;
                _field.OnValueChanged += action;
                if (doOnSubscribe) Trigger();
            }

            public void Dispose()
            {
                _field.OnValueChanged -= _fieldValueChangeAction;
            }

            public void Trigger()
            {
                _field.Value = _field.Value;
            }
        }
    }
}