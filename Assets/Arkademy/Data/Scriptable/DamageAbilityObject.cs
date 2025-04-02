using UnityEngine;

namespace Arkademy.Data.Scriptable
{
    [CreateAssetMenu(fileName = "New Damage Ability", menuName = "Data/Ability/DamageAbility", order = 0)]
    public class DamageAbilityObject : AbilityObject<Ability.DamageEffect>
    {
    }
}