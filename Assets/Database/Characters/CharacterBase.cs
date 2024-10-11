using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy
{
    [CreateAssetMenu(fileName = "New Character Base", menuName = "Data/Add Character Base", order = 0)]
    public class CharacterBase : ScriptableObject
    {
        public CharacterData baseData;
    }

    [Serializable]
    public struct CharacterData
    {
        [Serializable]
        public struct Attr
        {
            public enum Category
            {
                XpGain,
                Life,
                Stamina,
                Source,
                MoveSpeed,
                Luck,
                Strength,
                Constitution,
                Dexterity,
                Wisdom,
                Faith,
                Charisma,
            }

            public Category category;
            public int value;
        }

        [Serializable]
        public struct Mastery
        {
            public EquipmentData.Type equipType;
            public int value;
        }

        public Attr[] attributes;
        public Mastery[] masteries;
        public EquipmentSlot[] equipmentSlots;

        public int GetFinalAttrValue(Enum attr, Enum[] subAttrs)
        {
            var ret = 0;
            if (attributes!=null && attr is Attr.Category)
            {
                ret += attributes.FirstOrDefault(x => x.category == (Attr.Category)attr).value;
            }

            if (masteries!=null && attr is EquipmentData.Type)
            {
                ret += masteries.FirstOrDefault(x => x.equipType == (EquipmentData.Type)attr).value;
            }

            if (equipmentSlots.Length <= 0) return ret;
            foreach (var equipmentSlot in equipmentSlots)
            {
                var eq = equipmentSlot.equipment;
                if (eq.type == 0) continue;
                foreach (var affix in eq.affixes)
                {
                    if (affix.targetCategories.category.Equals(attr))
                    {
                        ret += affix.value;
                    }
                }
            }
            return ret;
        }

        public Dictionary<Enum, int> GetAllActiveAttributes()
        {
            var ret = new Dictionary<Enum, int>();
            foreach (var attr in attributes)
            {
                ret[attr.category] += attr.value;
            }

            foreach (var mastery in masteries)
            {
                ret[mastery.equipType] += mastery.value;
            }

            foreach (var equipmentSlot in equipmentSlots)
            {
                var eq = equipmentSlot.equipment;
                if(eq.type==0 || eq.affixes==null)continue;
                foreach (var affix in eq.affixes)
                {
                    ret[affix.targetCategories.category] = affix.value;
                }
            }
            return ret;
        }
    }
}