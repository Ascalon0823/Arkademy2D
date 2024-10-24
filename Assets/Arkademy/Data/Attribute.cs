using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Attribute : Field
    {
        
        public List<Modifier> persistentModifiers = new List<Modifier>();
        [JsonIgnore] [SerializeField] private List<Modifier> runtimeModifiers = new List<Modifier>();
        [JsonIgnore] public Func<long, Dictionary<Modifier.Type, long>, long> Calculator;
        private readonly Dictionary<Modifier, ISubscription> _subscriptions = new Dictionary<Modifier, ISubscription>();

        public Attribute(string key, long value) : base(key, value)
        {
        }

        public new Attribute Copy()
        {
            var attr = new Attribute(key, Value);
            attr.persistentModifiers = persistentModifiers.Select(x => x.Copy()).ToList();
            attr.runtimeModifiers = runtimeModifiers.Select(x => x.Copy()).ToList();
            attr.UpdateSubscriptions();
            return attr;
        }

        public void UpdateSubscriptions()
        {
            var modifiers = new List<Modifier>();
            modifiers.AddRange(persistentModifiers);
            modifiers.AddRange(runtimeModifiers);
            foreach (var modifier in modifiers)
            {
                if (_subscriptions.TryGetValue(modifier, out var sub) && sub != null) continue;
                _subscriptions[modifier] = modifier.Subscribe(x => Trigger(),false);
            }
            Trigger();
        }

        public void AddModifier(Modifier modifier, bool persistent = false)
        {
            var targetModifiers = persistent ? this.persistentModifiers : this.runtimeModifiers;
            targetModifiers.Add(modifier);
            _subscriptions.Add(modifier, modifier.Subscribe(x => Trigger(),false));
            Trigger();
        }

        public void RemoveModifier(Modifier modifier, bool persistent = false)
        {
            var targetModifiers = persistent ? this.persistentModifiers : this.runtimeModifiers;
            targetModifiers.Remove(modifier);
            if (_subscriptions.Remove(modifier, out var subscription))
            {
                subscription.Dispose();
            }

            Trigger();
        }

        private void Trigger()
        {
            OnValueChanged?.Invoke(GetValue());
        }

        private Dictionary<Modifier.Type, long> GetModifierValues()
        {
            var ret = new Dictionary<Modifier.Type, long>();
            var temp = new Dictionary<Modifier.Type, List<Modifier>>();
            foreach (var modifier in persistentModifiers)
            {
                if (!temp.TryGetValue(modifier.type, out var modifiers))
                {
                    modifiers = new List<Modifier>();
                    temp[modifier.type] = modifiers;
                }

                modifiers.Add(modifier);
            }

            foreach (var modifier in runtimeModifiers)
            {
                if (!temp.TryGetValue(modifier.type, out var modifiers))
                {
                    modifiers = new List<Modifier>();
                    temp[modifier.type] = modifiers;
                }

                modifiers.Add(modifier);
            }

            foreach (var pair in temp)
            {
                ret[pair.Key] = pair.Key.Calculate(pair.Value.Select(x => x.Value).ToArray());
            }

            return ret;
        }

        private Func<long, Dictionary<Modifier.Type, long>, long> _baseCalculation = (attrBase, modifiers) =>
        {
            var flat = modifiers.GetValueOrDefault(Modifier.Type.Flat, 0);
            var percent = modifiers.GetValueOrDefault(Modifier.Type.AdditivePercent, 0);
            var percentMulti = modifiers.GetValueOrDefault(Modifier.Type.MutiplicativePercent, 0);
            var percentNegate = modifiers.GetValueOrDefault(Modifier.Type.Chance, 10000);
            return (long)Math.Round((attrBase + flat) * ((10000 + percent) / 10000.0) *
                                    ((10000 + percentMulti) / 10000.0) * (percentNegate / 10000.0));
        };

        public long GetValue(Func<long, Dictionary<Modifier.Type, long>, long> calculation = null)
        {
            calculation ??= Calculator ?? _baseCalculation;
            return calculation?.Invoke(Value, GetModifierValues()) ?? 0;
        }
    }
}