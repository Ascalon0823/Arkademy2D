using System;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class AttributeProfile : Attribute
    {
        public string abbrev;
        public Sprite icon;
        public bool canInvest;
        public int returnRate;
        public int diminishingReturn;
        public int initialInvestReq;
        public int investReqIncrement;
        public int investmentBaseValue;
        public Modifier.Category investCalculationType;

        public int GetInvestLevel(int xp)
        {
            if (!canInvest) return 0;
            var calXp = (long)xp;
            return Mathf.FloorToInt((float)(Math.Sqrt(initialInvestReq * initialInvestReq + 8 * calXp * investReqIncrement) -
                                     initialInvestReq) / investReqIncrement / 2);
        }

        public int GetNextCost(int xp)
        {
            if (!canInvest) return 0;
            var level = GetInvestLevel(xp);
            return level * investReqIncrement + initialInvestReq;
        }

        public int GetPrevCost(int xp)
        {
            if (!canInvest) return 0;
            var level = GetInvestLevel(xp);
            if (level == 0) return 0;
            return initialInvestReq + (level - 1) * investReqIncrement;
        }

        public int GetInvestValue(int xp)
        {
            return returnRate * GetInvestLevel(xp) + investmentBaseValue;
        }
    }
}