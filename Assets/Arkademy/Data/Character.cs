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
    public class AttrInvestment : Investment
    {
        public Attribute.Type type;
    }

    [Serializable]
    public class AbilityInvestment
    {
        public string abilityName;
        public int addedLevel;
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

        public List<AbilityInvestment> abilityInvestments = new List<AbilityInvestment>();


        public bool AddToInventory(Item item)
        {
            inventory.Add(item);
            return true;
        }

        public List<string> GetAllAbilities()
        {
            var race = Race.GetRace(raceName);
            return race.abilities.Select(x => x.abilityName).ToList();
        }

        public List<string> GetAvailableAbilities()
        {
            var race = Race.GetRace(raceName);
            return race.abilities
                .Where(x => x.beginningLevel +
                    (abilityInvestments.FirstOrDefault(z => z.abilityName == x.abilityName)?.addedLevel ?? 0) > 0)
                .Select(x => x.abilityName).ToList();
        }

        public List<AttributeProfile> GetAllAttributeProfiles()
        {
            var race = Race.GetRace(raceName);
            return race.attributes;
        }

        public int GetAbilityLevel(string abilityName)
        {
            var race = Race.GetRace(raceName);
            var availability = race.abilities.FirstOrDefault(x => x.abilityName == abilityName);
            if (availability == null) return 0;
            var investment = abilityInvestments.FirstOrDefault(x => x.abilityName == abilityName);
            return availability.beginningLevel + (investment?.addedLevel ?? 0);
        }
    }
}