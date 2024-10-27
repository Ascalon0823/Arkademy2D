using System;
using Arkademy.Data;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
namespace Arkademy.Configs
{
    [CreateAssetMenu(fileName = "Affix Config", menuName = "Config/Affix Config", order = 0)]
    public class AffixConfig : ScriptableObject
    {
        [Serializable]
        public class AffixConfigData
        {
            public Affix affix;
            [Serializable]
            public class Range
            {
                public long min;
                public long max;
            }
            public EquipmentSlot.Category category;
            public List<Range> ranges;
        }
        public List<AffixConfigData> affixConfigs;
    }
}