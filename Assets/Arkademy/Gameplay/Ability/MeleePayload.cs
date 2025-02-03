using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class MeleePayload : AbilityPayload
    {
        public Trigger trigger;
        public override void Init(AbilityEventData data, AbilityBase parent, float dura)
        {
            base.Init(data, parent,dura);
            transform.SetParent(parent.transform);
            transform.localPosition = Vector2.zero;
            transform.up = data.TryGetDirection(parent.user.transform.position, out var dir) ? dir : parent.user.facing;
            transform.localScale = new Vector3(parent.GetRange(), parent.GetRange());
            trigger.OnTrigger.AddListener(c =>
            {
                if (c.GetCharacter(out var chara) && chara.faction != parent.user.faction)
                {
                    chara.TakeDamage(100);
                }
            });
        }
        
    }
}