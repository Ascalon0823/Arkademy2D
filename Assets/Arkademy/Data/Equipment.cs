using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class EquipmentSlot
    {
        public enum Category
        {
            MainHand,
            OffHand,
            Head,
            Shoulder,
            Body,
            Hand,
            Feet,
            Face,
            Accessory
        }

        public Category category;
        public Equipment equipment;
        public Action<Equipment, Equipment> OnEquipmentChanged;

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