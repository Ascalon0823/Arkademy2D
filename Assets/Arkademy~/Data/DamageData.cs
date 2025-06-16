using System;
using System.Linq;

namespace Arkademy.Data
{
    [Serializable]
    public class DamageData
    {
        public long[] amounts;
        public Character dealer;

        public DamageData()
        {
            
        }
        public DamageData(long newAmount)
        {
            amounts = new []{newAmount};
        }

        public DamageData(long[] newAmounts)
        {
            amounts = newAmounts;
        }

        public long Sum()
        {
            return amounts.Sum();
        }
    }
}