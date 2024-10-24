using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public struct EquipmentSlot
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
        
        public Equipment(Equipment baseEquipment)
        {
            name = baseEquipment.name;
            templateName = baseEquipment.templateName;
            slotCategory = baseEquipment.slotCategory;
            attributes = new List<Attribute>();
            attributes.AddRange(baseEquipment.attributes);
            requirements = new List<Requirement>();
            requirements.AddRange(baseEquipment.requirements);
            affixesWhenEquip = new List<Affix>();
            affixesWhenEquip.AddRange(baseEquipment.affixesWhenEquip);
            BuildAttrCache();
        }
    }
}