using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Data
{
    public class Formula
    {
        public static float Roll(int luck)
        {
            var negative = luck < 0;
            luck = Mathf.Abs(luck);
            var extraRoll = luck / 100 + (luck % 100 > Random.Range(0, 100) ? 1 : 0);
            var rolls = new List<int>();
            for (var i = 0; i < 1 + extraRoll; i++)
            {
                rolls.Add(Random.Range(0, 100));
            }
            var final = negative ? rolls.Min() : rolls.Max();
            return final / 100f;
        }

        [Serializable]
        public class OffensiveData
        {
            public long atk;
            public long mastery = 5000;
            public long ability = 10000;
            public long statScaling;
            public long damageBuff;
            public long criticalDamage;
            public long resistancePenetration;
        }

        [Serializable]
        public class DefensiveData
        {
            public long damageResistance;
        }
        public static long CalculateDamage(OffensiveData offensive, DefensiveData defensive)
        {
            var dealt = offensive.atk * (offensive.mastery / 10000f)
                                      * (offensive.ability / 10000f) * (1 + offensive.statScaling / 100f)
                                      * (1 + offensive.damageBuff / 10000f) * (1 + offensive.criticalDamage / 10000f);
            var received = dealt *
                           (1 - defensive.damageResistance / 10000f * (1 - offensive.resistancePenetration / 10000f));
            return Mathf.RoundToInt(Mathf.Max(0,received));
        }
    }
}