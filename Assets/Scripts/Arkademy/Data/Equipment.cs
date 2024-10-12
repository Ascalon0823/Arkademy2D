using System;
using System.Collections.Generic;

namespace Arkademy.Data
{
    [Serializable]
    public struct EquipmentSlot
    {
        public string type;
    }
    
    [Serializable]
    public struct Equipment
    {
        public string name;
        public List<EquipmentSlot> equipableSlots;
        public List<Attribute> attributes;
        public List<Requirement> requirements;
        public List<Affix> affixes;
    }
}