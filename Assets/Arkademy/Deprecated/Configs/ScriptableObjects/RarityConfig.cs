using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Configs
{
    [CreateAssetMenu(fileName = "Item Rarity Config", menuName = "Config/Item Rarity Config", order = 0)]
    public class RarityConfig : ScriptableObject
    {
        private static RarityConfig _instance;

        public static RarityConfig Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<RarityConfig>("Item Rarity Config");
                }

                return _instance;
            }
        }

        [Serializable]
        public struct ItemRarityConfig
        {
            public int rarity;
            public Color color;
        }

        [SerializeField] private List<ItemRarityConfig> itemRarityConfig;

        private readonly Dictionary<int, ItemRarityConfig> _configCache = new Dictionary<int, ItemRarityConfig>();

        private void UpdateCache()
        {
            foreach (var config in itemRarityConfig)
            {
                _configCache[config.rarity] = config;
            }
        }
        public bool TryGetConfig(int rarity, out ItemRarityConfig config)
        {
            if(!_configCache.Any())UpdateCache();
            return _configCache.TryGetValue(rarity, out config);
        }

        private void OnEnable()
        {
            _instance = this;
        }
    }
}