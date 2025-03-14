using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class RangeBaseAttack : AbilityBase
    {
        public override float GetRange()
        {
            return user.Attributes.Get(Attribute.Type.Range);
        }

        public override float GetUseTime()          
        {
            return 1f/user.Attributes.Get(Attribute.Type.AttackSpeed);
        }
    }
}