using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Data.Deprecate.Config
{
    [CreateAssetMenu(fileName = "New Equipment Roll Config", menuName = "Data/Config/Equipment Roll Config", order = 0)]
    public class EquipmentRollConfig : ScriptableObject
    {
        [Serializable]
        public struct RarityConfig
        {
            public EquipmentData.Rarity rarity;
            public int minRollReq;
            public int minAffixCount;
            public int maxAffixCount;
        }

        [SerializeField] protected RarityConfig[] rarityConfigs;

        public static EquipmentRollConfig Get()
        {
            return Resources.Load<EquipmentRollConfig>("EquipmentRollConfig");
        }

        public int GetAffixCount(int roll,out EquipmentData.Rarity rarity)
        {
            rarity = EquipmentData.Rarity.Normal;
            foreach (var config in rarityConfigs.OrderByDescending(x => x.minRollReq))
            {
                if (roll >= config.minRollReq)
                {
                    rarity = config.rarity;
                    return Random.Range(config.minAffixCount, config.maxAffixCount + 1);
                }
            }

            return 0;
        }
    }
}