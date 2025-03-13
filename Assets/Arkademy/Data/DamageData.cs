using System;

namespace Arkademy.Data
{
    [Serializable]
    public class DamageData
    {
        public long amount;
        public Character dealer;

        public DamageData()
        {
            
        }
        public DamageData(long newAmount)
        {
            amount = newAmount;
        }
    }
}