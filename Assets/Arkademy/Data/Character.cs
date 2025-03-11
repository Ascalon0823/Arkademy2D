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

        [JsonIgnore] private Dictionary<Attribute.Type, Attribute> _attributes;

        [JsonIgnore]
        public Dictionary<Attribute.Type, Attribute> attributes
        {
            get
            {
                if (_attributes == null) SetupAttribute();
                return _attributes;
            }
        }

        public Attribute this[Attribute.Type t]
        {
            get
            {
                if (_attributes == null)
                {
                    SetupAttribute();
                }

                return _attributes.GetValueOrDefault(t, null);
            }
        }

        public void SetCurr(Attribute.Type t, int current)
        {
            var attr = this[t];
            if (attr != null && attr.IsResource())
            {
                attr.current = current;
            }
        }

        public float Get(Attribute.Type t, float defaultValue = 0)
        {
            return this[t]?.Value() ?? defaultValue;
        }

        public int GetBase(Attribute.Type t, int defaultValue = 0)
        {
            return this[t]?.BaseValue() ?? defaultValue;
        }

        public float GetCurr(Attribute.Type t, float defaultValue = 0)
        {
            return this[t]?.Curr() ?? defaultValue;
        }

        public int GetBaseCurr(Attribute.Type t, int defaultValue = 0)
        {
            return this[t]?.BaseCurr() ?? defaultValue;
        }

        private void SetupAttribute()
        {
            _attributes = new Dictionary<Attribute.Type, Attribute>();
            var race = Race.GetRace(raceName);
            foreach (var attr in race.attributes)
            {
                _attributes[attr.type] = attr.Copy();
            }
        }

        public Equipment GetEquipment(int slotIdx)
        {
            return equipmentSlots.FirstOrDefault(x => x.slot == slotIdx)?.equipment;
        }

        public void Equip(Equipment equipment)
        {
            var baseData = ItemBase.GetItemBase(equipment.baseName);
            if (baseData == null) return;
            var slot = equipmentSlots.FirstOrDefault(x => x.slot == baseData.slot);
            if (slot == null) return;
            if (slot.equipment != null)
            {
                UnEquip(slot);
            }
            slot.equipment = equipment;
            Setup(slot);
        }

        public void Setup(EquipmentSlot slot)
        {
            if (slot == null) return;
            foreach (var mod in slot.currentModifiers)
            {
                this[mod.attrType]?.RemoveMod(mod);
            }
            slot.currentModifiers.Clear();
            if (slot.equipment == null)
            {
                return;
            }
           
            var baseData = ItemBase.GetItemBase(slot.equipment.baseName);
            var modifiers = new List<Attribute.Modifier>();
            modifiers.AddRange(baseData.equipmentModifiers);
            modifiers.AddRange(slot.equipment.additional);
            foreach (var mod in modifiers)
            {
                var instance = mod.Copy();
                slot.currentModifiers.Add(instance);
                this[mod.attrType]?.AddMod(instance);
            }
        }

        public void UnEquip(EquipmentSlot slot)
        {
            if (slot?.equipment == null) return;

            slot.equipment = null;
            Setup(slot);
        }
    }
}