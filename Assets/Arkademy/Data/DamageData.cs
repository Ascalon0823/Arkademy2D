using System;

namespace Arkademy.Data
{
    [Serializable]
    public class DamageData
    {
        public int amount;
        public Character dealer;

        public DamageData()
        {
            
        }
        public DamageData(int newAmount)
        {
            amount = newAmount;
        }
    }
}