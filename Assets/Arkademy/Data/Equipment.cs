using System;
using System.Collections.Generic;
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
    public class Equipment
    {
        [Serializable]
        public struct Attribute
        {
            public static Attribute strength => new() { key = "Strength", value = 10 };
            public string key;
            public int value;
        }
        public string templateName;
        public string name;
        public EquipmentSlot.Category slotCategory;
        public List<Attribute> attributes;
        public List<Requirement> requirements;
        public List<Affix> affixes;

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
            affixes = new List<Affix>();
            affixes.AddRange(baseEquipment.affixes);
        }
    }
}