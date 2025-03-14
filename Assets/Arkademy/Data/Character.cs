using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Arkademy.Data
{
    [Serializable]
    public class Investment
    {
        public int xp;
    }

    [Serializable]
    public class AttrInvestment:Investment
    {
        public Attribute.Type type;
    }
    [Serializable]
    public class Character
    {
        public string displayName;
        public string raceName;
        public int xp;
        public int gold;
        public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
        public int clearedRift;
        public List<AttrInvestment> attrInvestments = new List<AttrInvestment>();
        public List<Item> inventory = new List<Item>();

      

        public bool AddToInventory(Item item)
        {
            inventory.Add(item);
            return true;
        }
    }
}