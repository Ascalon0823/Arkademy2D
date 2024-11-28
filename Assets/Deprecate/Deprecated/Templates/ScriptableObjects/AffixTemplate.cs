using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Templates
{
    [Serializable]
    public class AffixConfig
    {
        public EquipmentSlot.Category categories;
        public RarityConfig[] rarities;

        [Serializable]
        public class RarityConfig
        {
            public int rarity;
            public long minValue;
            public long maxValue;
        }
    }

    public abstract class AffixTemplate : ScriptableObject
    {
        public Data.Affix affixData;
        public AffixConfig[] config;

        private void OnEnable()
        {
            affixData.templateName = name;
        }

        public virtual Affix GetAffix(int luck)
        {
            return GetAffix(Formula.Roll(luck));
        }

        public virtual Affix GetAffix()
        {
            return GetAffix(Random.Range(0, 100) / 100f);
        }

        public virtual Affix GetAffix(float value, EquipmentSlot.Category category = EquipmentSlot.Category.MainHand,
            int rarity = 0)
        {
            var affix = affixData.Copy();
            affix.effects??=new List<Data.Effect>();
            affix.value = value;
            return affix;
        }
    }

    public abstract class AffixTemplate<T> : AffixTemplate where T : Data.Effect
    {
        public T effect;
    }
}