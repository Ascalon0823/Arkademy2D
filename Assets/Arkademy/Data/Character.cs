using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Character
    {
        [HideInInspector] public string templateName;
        public string name;
        public List<Attribute> attributes;
        public List<Attribute> apAllocation;
        public List<EquipmentSlot> slots;

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

        public Character()
        {
        }

        public Character(Character baseChar)
        {
            name = baseChar.name;
            templateName = baseChar.templateName;
            attributes = new List<Attribute>();
            attributes.AddRange(baseChar.attributes);
            apAllocation = new List<Attribute>();
            apAllocation.AddRange(baseChar.apAllocation);
            slots = new List<EquipmentSlot>();
            slots.AddRange(baseChar.slots);
        }
    }
}