using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class EquipmentSlot
    {
        [Flags]
        public enum Category
        {
            MainHand=1<<0,
            OffHand=1<<1,
            Head=1<<2,
            Shoulder=1<<3,
            Body=1<<4,
            Hand=1<<5,
            Feet=1<<6,
            Face=1<<7,
            Accessory=1<<8,
            Utility=1<<9,
        }

        public Category category;
        public Equipment equipment;
        [JsonIgnore]public Action<Equipment, Equipment> OnEquipmentChanged;

        public bool HasEquipment()
        {
            return equipment != null && !string.IsNullOrEmpty(equipment.templateName);
        }
    }

    [Serializable]
    public class Equipment : Item
    {
        public EquipmentSlot.Category slotCategory;
        public List<Attribute> attributes;
        public List<Affix> affixesWhenEquip;
        private Dictionary<string, Attribute> _attrCache = new Dictionary<string, Attribute>();

        private void BuildAttrCache()
        {
            foreach (var attr in attributes)
            {
                _attrCache[attr.key] = attr;
            }
        }

        public bool TryGetAttr(string attrName, out Attribute attribute)
        {
            if (!_attrCache.Any()) BuildAttrCache();
            return _attrCache.TryGetValue(attrName, out attribute);
        }

        public Equipment()
        {
        }

        public Equipment Copy()
        {
            var newEquip = new Equipment
            {
                name = name,
                templateName = templateName,
                slotCategory = slotCategory,
                rarity = rarity,
                stackLimit = stackLimit,
                attributes = new List<Attribute>(),
                affixesWhenEquip = new List<Affix>()
            };
            attributes.AddRange(attributes);
            affixesWhenEquip.AddRange(affixesWhenEquip);
            newEquip.BuildAttrCache();
            return newEquip;
        }
    }
}