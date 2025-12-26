using Arkademy2D.Core.Models;
using UnityEngine;
namespace Arkademy2D.Game.Data.Static.Item
{
    public class WeaponBase : ItemExtraProviderStaticData<WeaponBase>
    {
        public int damage;
        public float range;
        public override ItemExtra GetDefaultItemExtra()
        {
            return new Weapon
            {
                damage = damage
            };
        }
    }
}