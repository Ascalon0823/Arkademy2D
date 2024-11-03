using System;

namespace Arkademy.Data
{
    [Serializable]
    public class Ammunition : Item
    {
        [Flags]
        public enum AmmunitionType{
            Arrow=1<<0,
            Bullet=1<<1, 
            Launch=1<<2,
        }

        public AmmunitionType type;
        public long travelSpeed;
        public DamageEvent damagePercentage;
    }
}