using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Character
    {
        [HideInInspector]public string templateName;
        public string name;
        public List<Attribute> attributes;
        public List<EquipmentSlot> slots;

        public Character()
        {
            
        }
        public Character(Character baseChar)
        {
            name = baseChar.name;
            templateName = baseChar.templateName;
            attributes = new List<Attribute>();
            attributes.AddRange(baseChar.attributes);
            slots = new List<EquipmentSlot>();
            slots.AddRange(baseChar.slots);
        }
    }
}