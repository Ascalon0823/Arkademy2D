using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class ChannelAbility : AbilityBase
    {
        public override void Use(AbilityEventData eventData)
        {
            if (currentPayload)
            {
                currentPayload.UpdatePayload(eventData);
                return;
            }

            currentPayload = Instantiate(payloadPrefab);
            currentPayload.Init(eventData, this);
            inUse = true;
        }

        public override void Cancel()
        {
            remainingCooldown = GetCooldown();
            inUse = false;
            currentPayload = null;
        }
    }
}