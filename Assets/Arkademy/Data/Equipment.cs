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
        [HideInInspector]public string templateName;
        public string name;
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
            attributes = new List<Attribute>();
            attributes.AddRange(baseEquipment.attributes);
            requirements = new List<Requirement>();
            requirements.AddRange(baseEquipment.requirements);
            affixes = new List<Affix>();
            affixes.AddRange(baseEquipment.affixes);
        }
    }
}