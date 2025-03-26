using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;

namespace Arkademy.Gameplay
{
    public class Attributes
    {
        private readonly Dictionary<Attribute.Type, Attribute> _attrs = new();
        private readonly Dictionary<AttrInvestment, Attribute.Modifier> _investmentModifiers = new();
        private readonly Dictionary<EquipmentSlot, List<Attribute.Modifier>> _equippedModifiers = new();

        public List<Attribute> GetAll()
        {
            return _attrs.Select(x => x.Value).ToList();
        }
        public Attributes(Race race, Data.Character character)
        {
            Setup(race,character);
        }
        private void Setup(Race race, Data.Character character)
        {
            UpdateRace(race);
            foreach (var investment in character.attrInvestments)
            {
                UpdateInvestment(investment, race.attributes.FirstOrDefault(x=>x.type==investment.type));
            }

            foreach (var equipmentSlot in character.equipmentSlots)
            {
                UpdateEquipment(equipmentSlot);
            }

            foreach (var attr in _attrs)
            {
                attr.Value.current = attr.Value.BaseValue();
            }
        }

        public void UpdateRace(Race race)
        {
            foreach (var attributeProfile in race.attributes)
            {
                if (!_attrs.TryGetValue(attributeProfile.type, out var attr))
                {
                    attr = attributeProfile.Copy();
                    _attrs[attributeProfile.type] = attr;
                }

                attr.value = attributeProfile.value;
                attr.calType = attributeProfile.calType;
            }
        }

        public void UpdateInvestment(AttrInvestment attrInvestment, AttributeProfile profile)
        {
            if (!_attrs.TryGetValue(attrInvestment.type, out var attr))
            {
                return;
            }

            if (!_investmentModifiers.TryGetValue(attrInvestment, out var mod))
            {
                mod = new Attribute.Modifier
                {
                    attrType = attr.type,
                    category = profile.investCalculationType,
                    value = profile.investmentBaseValue
                };
                _investmentModifiers[attrInvestment] = mod;
                attr.AddMod(mod);
            }

            mod.value = profile.GetInvestValue(attrInvestment.xp);
        }

        public void UpdateEquipment(EquipmentSlot slot)
        {
            if (!_equippedModifiers.TryGetValue(slot, out var list))
            {
                list = new List<Attribute.Modifier>();
                _equippedModifiers[slot] = list;
            }

            foreach (var mod in list)
            {
                if (!_attrs.TryGetValue(mod.attrType, out var attr))
                {
                    continue;
                }

                attr.RemoveMod(mod);
            }

            list.Clear();
            foreach (var mod in slot.equipment.GetAllMods())
            {
                if (!_attrs.TryGetValue(mod.attrType, out var attr))
                {
                    continue;
                }

                attr.AddMod(mod);
                list.Add(mod);
            }
        }

        public void AddMod(Attribute.Modifier mod)
        {
            if (_attrs.TryGetValue(mod.attrType, out var attr))
            {
                attr.AddMod(mod);
            }
        }

        public void RemoveMod(Attribute.Modifier mod)
        {
            if (_attrs.TryGetValue(mod.attrType, out var attr))
            {
                attr.RemoveMod(mod);
            }
        }

        public float Get(Attribute.Type type)
        {
            if (_attrs.TryGetValue(type, out var attr))
            {
                return attr.Value();
            }
            return default;
        }

        public long GetBase(Attribute.Type type)
        {
            if (_attrs.TryGetValue(type, out var attr))
            {
                return attr.BaseValue();
            }
            return default;
        }

        public float GetCurr(Attribute.Type type)
        {
            if (_attrs.TryGetValue(type, out var attr))
            {
                return attr.Curr();
            }
            return default;
        }
        public long GetBaseCurr(Attribute.Type type)
        {
            if (_attrs.TryGetValue(type, out var attr))
            {
                return attr.BaseCurr();
            }
            return default;
        }
        
        public Attribute this[Attribute.Type t]
        {
            get
            {
                return _attrs.GetValueOrDefault(t, null);
            }
        }
    }
}