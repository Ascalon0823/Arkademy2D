using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class RangeBaseAttack : AbilityBase
    {
        public Item GetAmmunition()
        {
            return user.data.inventory.FirstOrDefault(x =>
            {
                var baseItem = ItemBase.GetItemBase(x.baseName);
                if (!baseItem.tags.tags.Contains("Ammunition")) return false;
                return x.stack > 0;
            });
        }
        public override float GetRange()
        {
            return user.Attributes.Get(Attribute.Type.Range);
        }

        public override float GetUseTime()          
        {
            return 1f/user.Attributes.Get(Attribute.Type.AttackSpeed);
        }

        public override bool CanUse(AbilityEventData eventData)
        {
            return base.CanUse(eventData) && GetAmmunition() != null;
        }
    }
}