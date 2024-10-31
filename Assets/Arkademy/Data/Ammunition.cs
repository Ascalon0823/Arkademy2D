using System;

namespace Arkademy.Data
{
    [Serializable]
    public class Ammunition : Item
    {
        public long travelSpeed;
        public DamageEvent damagePercentage;
    }
}