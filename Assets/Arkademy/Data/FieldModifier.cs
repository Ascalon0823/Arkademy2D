using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class FieldModifiers
    {
        public enum ModifyType
        {
            Flat,
            Percent
        }

        [Serializable]
        public class FieldModifier
        {
            public ModifyType type;
            public string key;
            public long value;
        }

        [SerializeField] private ReactiveFields flatModifiers = new ReactiveFields();
        [SerializeField] private ReactiveFields percentModifiers = new ReactiveFields();
        private Dictionary<string, List<FieldModifier>> _fieldModifiers = new Dictionary<string, List<FieldModifier>>();

        public List<ReactiveFields.Field.Handle> Subscribe(ReactiveFields.Field field,
            Action<long, long, long> onFinalValueUpdated, bool doOnSubscribe = true)
        {
            if (!flatModifiers.TryGet(field.key, out var flatF))
            {
                flatF = new ReactiveFields.Field { key = field.key, Value = 0 };
                flatModifiers.Fields.Add(flatF);
                flatModifiers.BuildCache();
            }

            if (!percentModifiers.TryGet(field.key, out var percentF))
            {
                percentF = new ReactiveFields.Field { key = field.key, Value = 0 };
                percentModifiers.Fields.Add(percentF);
                percentModifiers.BuildCache();
            }

            var flatHandle = flatF.Subscribe((_, x) => onFinalValueUpdated?.Invoke(field.Value, x, percentF.Value),
                false);
            var percentHandle =
                percentF.Subscribe((_, x) => onFinalValueUpdated?.Invoke(field.Value, flatF.Value, x), false);
            var fieldHandle = field.Subscribe((_, x) => onFinalValueUpdated?.Invoke(x, flatF.Value, percentF.Value),
                false);
            if (doOnSubscribe) fieldHandle.ForceTrigger();
            return new List<ReactiveFields.Field.Handle> { flatHandle, percentHandle, fieldHandle };
        }

        public void AddModifier(FieldModifier modifier)
        {
            if (!_fieldModifiers.TryGetValue(modifier.key, out List<FieldModifier> modifiers))
            {
                modifiers = new List<FieldModifier>();
                _fieldModifiers.Add(modifier.key, modifiers);
            }

            modifiers.Add(modifier);
            var value = modifiers.Where(x => x.type == modifier.type).Sum(x => x.value);
            var targetFields = modifier.type == ModifyType.Flat ? flatModifiers : percentModifiers;
            if (!targetFields.TryGet(modifier.key, out var field))
            {
                field = new ReactiveFields.Field
                {
                    key = modifier.key,
                    Value = 0
                };
                targetFields.Fields.Add(field);
                targetFields.BuildCache();
            }

            field.Value = value;
        }

        public void RemoveModifier(FieldModifier modifier)
        {
            if (!_fieldModifiers.TryGetValue(modifier.key, out List<FieldModifier> modifiers))
            {
                return;
            }

            modifiers.Remove(modifier);
            var value = modifiers.Where(x => x.type == modifier.type).Sum(x => x.value);
            var targetFields = modifier.type == ModifyType.Flat ? flatModifiers : percentModifiers;
            if (!targetFields.TryGet(modifier.key, out var field))
            {
                return;
            }

            field.Value = value;
        }
    }
}