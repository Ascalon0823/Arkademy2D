using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public partial class Character
    {
        [HideInInspector] public string templateName;
        public string name;
        public Progression progression;
        public Growth growth;
        public List<EquipmentSlot> slots;


        public Character Copy()
        {
            return new Character
            {
                templateName = templateName,
                name = name,
                progression = progression.Copy(),
                growth = growth.Copy(),
                attributes = attributes.ToList(),
                apAllocation = apAllocation.ToList(),
                slots = slots.ToList()
            };
        }

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
        public class Growth : ReactiveFields
        {
            [SerializeField] private ReactiveFields origin = new();

            public ReactiveFields Origin
            {
                get => origin;
                set
                {
                    origin = value;
                    foreach (var f in origin.Fields)
                    {
                        AddField(f);
                    }
                }
            }

            private void AddField(Field field)
            {
                if (TryGet(field.key, out _)) return;
                field = field.Copy();
                field.Value = 0;
                Fields.Add(field);
                _valueCache[field.key] = field;
            }
            public new Growth Copy()
            {
                return new Growth
                {
                    Origin = Origin.Copy()
                };
            }
        }

        #region Deprecate

        public List<Attribute> attributes;
        public List<Attribute> apAllocation;

        public bool TryGetAttribute(string key, out Attribute attr, out Attribute allocated)
        {
            attr = default;
            allocated = default;
            if (attributes == null) return false;
            attr = attributes.FirstOrDefault(x => x.key == key);
            if (apAllocation != null)
            {
                allocated = apAllocation.FirstOrDefault(x => x.key == key);
            }

            return !string.IsNullOrEmpty(attr.key);
        }

        public bool TryUpdateAttribute(string key, int value)
        {
            if (!attributes.Any(x => x.key == key)) return false;
            var idx = attributes.FindIndex(x => x.key == key);
            attributes[idx] = new Attribute { key = key, value = value };
            return true;
        }

        public bool TryUpdateAllocation(string key, int value)
        {
            if (!attributes.Any(x => x.key == key)) return false;
            var idx = apAllocation.FindIndex(x => x.key == key);
            if (idx == -1) apAllocation.Add(new Attribute { key = key, value = value });
            else apAllocation[idx] = new Attribute { key = key, value = value };
            return true;
        }

        #endregion
    }
}