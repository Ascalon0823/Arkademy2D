using System;
using System.Collections.Generic;
using Arkademy.Data.Config;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Data
{
    [CreateAssetMenu(fileName = "New Equipment Base", menuName = "Data/Add Equipment Base", order = 0)]
    public class EquipmentBase : ScriptableObject
    {
        [SerializeField] protected EquipmentData baseData;
        [SerializeField] protected Sprite icon;

        [SerializeField] protected ExtraAffixes[] extraAffixBases;

        [Serializable]
        protected struct ExtraAffixes
        {
            public AffixBase affixBase;
            public int fixedValue;
        }

        public EquipmentData Generate()
        {
            var ret = baseData;
            var rarityRoll = Random.Range(0, 10000);
            var affixCount = EquipmentRollConfig.Get().GetAffixCount(rarityRoll, out var rarity);
            ret.dataObjectName = name;
            ret.rarity = rarity;
            ret.capacity = affixCount;
            var affixes = new List<AffixData>();
            var candidates = AffixBase.GetAffixCandidatesFor(ret);
            Debug.Log(JsonConvert.SerializeObject(candidates,Formatting.Indented));
            if (extraAffixBases != null)
            {
                foreach (var extra in extraAffixBases)
                {
                    if (!extra.affixBase.TryGetAffixFor(ret, out var affix)) continue;
                    if (extra.fixedValue != 0)
                    {
                        affix.value = extra.fixedValue;
                    }

                    affixes.Add(affix);
                }
            }

            var remainingAffixes = affixCount - affixes.Count;
            if (affixes.Count < affixCount && candidates.Length > 0)
            {
                for (var i = 0; i < remainingAffixes; i++)
                {
                    if (candidates[Random.Range(0, candidates.Length)].TryGetAffixFor(ret, out var affix))
                    {
                        affixes.Add(affix);
                    }
                }
            }

            ret.affixes = affixes.ToArray();
            return ret;
        }

        public static EquipmentBase GetBase(string baseName)
        {
            return Resources.Load<EquipmentBase>(baseName);
        }
    }

    [Serializable]
    public class EquipmentSlot
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
        public EquipmentData equipment;
    }

    [Serializable]
    public struct EquipmentData
    {
        [Flags]
        public enum Type
        {
            OneHandWeapon = 1 << 0,
            TwoHandWeapon = 1 << 1,
            OffHandWeapon = 1 << 2,
            Shield = 1 << 3,
            Helmet = 1 << 4,
            Cloak = 1 << 5,
            BodyArmor = 1 << 6,
            Glove = 1 << 7,
            Shoe = 1 << 8,
            Mask = 1 << 9,
            Accessory = 1 << 10
        }

        public enum Rarity
        {
            Normal,
            Magical,
            Rare,
            Mythical,
            Unique
        }

        [Serializable]
        public struct Requirements
        {
            public int levelMin;
            public CharaAttrReq[] attrReqs;
        }

        [Serializable]
        public struct CharaAttrReq
        {
            public CharacterData.Attr.Category category;
            public int minValue;
        }

        [Serializable]
        public struct CharaMasteryReq
        {
            public EquipmentData.Type type;
            public int minValue;
        }

        [Serializable]
        public struct Attr
        {
            public enum Category
            {
                PhysicalAttack,
                BaseSpeed,
                CriticalChance,
                CriticalDamage,
                Accuracy,
                KnockBack,
                PhysicalResistance,
                Evade,
                Stance,
            }

            public Category category;
            public int value;
        }

        [HideInInspector] public string dataObjectName;
        public string name;
        public string description;
        public Type type;
        public Rarity rarity;
        public Attr[] attributes;
        public Requirements requirements;
        public int capacity;
        public AffixData[] affixes;
    }
}