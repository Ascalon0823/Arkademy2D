using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Arkademy.Data.Deprecate
{
    public abstract class AffixBase : ScriptableObject {
        [SerializeField] protected string description;
        [SerializeField] protected int cost;
        [Serializable]
        protected struct AffixConfig
        {
            [Serializable]
            public struct RarityConfig
            {
                public EquipmentData.Rarity rarity;
                public int minValue;
                public int maxValue;
            }

             public EquipmentData.Type type;
            public RarityConfig[] rarityConfigs;
        }

        [SerializeField] protected AffixConfig[] configs;

        public static AffixBase[] GetAffixCandidatesFor(EquipmentData data)
        {
            return Resources.LoadAll<AffixBase>("")
                .Where(x => x.ValidFor(data, out _)).ToArray();
        }

        protected bool ValidFor(EquipmentData data, out AffixConfig.RarityConfig rarityConfig)
        {
            rarityConfig = default;
            if (configs == null || configs.Length == 0) return false;
            var validConfigs = configs.Where(x => (x.type & data.type) == data.type).ToList();
            if (validConfigs.Count <= 0) return false;
            var config = validConfigs[0];
            var rarities = config.rarityConfigs.Where(x => x.rarity == data.rarity).ToList();
            if (rarities.Count <= 0) return false;
            rarityConfig = rarities[0];
            return true;

        }

        protected abstract AffixData.TargetCategories GetCategories();
        public bool TryGetAffixFor(EquipmentData data, out AffixData affix)
        {
            affix = default;
            if (!ValidFor(data, out var rarityConfig)) return false;
            affix.targetCategories = GetCategories();
            affix.value = Random.Range(rarityConfig.minValue, rarityConfig.maxValue + 1);
            affix.cost = cost;
            return true;
        }
    }

   
    [Serializable]
    public struct AffixData
    {
        public enum Category
        {
            CharacterAttrBoost,
            MasteryBoost,
            EquipmentAttrBoost
        }
        [Serializable]
        public class TargetCategories
        {
            public Category category;
            public int categoryValue;
        }
        
        public TargetCategories targetCategories;
        public int value;
        public int cost;
    }
}