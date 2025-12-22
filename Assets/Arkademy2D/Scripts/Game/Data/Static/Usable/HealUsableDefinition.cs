using Arkademy2D.Game.Data.Runtime;
using UnityEngine;
namespace Arkademy2D.Game.Data.Static.Usable
{
    [CreateAssetMenu(fileName = "Heal Usable Effect", menuName = "Static/Usable/HealEffect", order = 0)]
    public class HealUsableDefinition:UsableEffectDefinition
    {
        public int healAmount;
        public override void UseEffect(UseContext context)
        {
            Debug.Log($"Heal {healAmount} on {context.userTransform.name}", context.userTransform);
            context.character.health.current += healAmount;
        }
    }
}