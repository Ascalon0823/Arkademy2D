using Arkademy.Data;

namespace Arkademy.Gameplay.Ability
{
    public class WeaponBaseAttack : AbilityBase
    {
        public AbilityPayload payloadPrefab;
        public override float GetRange()
        {
            return user.Attributes.Get(Attribute.Type.Range);
        }

        public override float GetUseTime()
        {
            return 1f / user.Attributes.Get(Attribute.Type.AttackSpeed);
        }

        public override void Use(AbilityEventData eventData, bool canceled = false)
        {
            base.Use(eventData, canceled);
            var payload = Instantiate(payloadPrefab);
            payload.Init(eventData, this, GetUseTime(), 0.25f * GetUseTime(), null);
        }
    }
}