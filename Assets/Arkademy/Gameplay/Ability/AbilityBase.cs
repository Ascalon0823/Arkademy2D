using System;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public struct AbilityEventData
    {
        public Character PrimaryTarget;
        public Character[] OtherTargets;
        public Vector2? Direction;
        public Vector2? Position;

        public bool TryGetPosition(out Vector2 position)
        {
            
            var result = Position ?? PrimaryTarget?.transform.position;
            position = result ?? Vector2.zero;
            return result.HasValue;
        }

        public bool TryGetDirection(Vector2 pos, out Vector2 direction)
        {
            direction = Vector2.zero;
            if (Direction.HasValue)
            {
                direction = Direction.Value;
                return true;
            }

            if (Position.HasValue)
            {
                direction = Position.Value - pos;
                return true;
            }

            if (PrimaryTarget)
            {
                direction = PrimaryTarget.transform.position - (Vector3)pos;
                return true;
            }

            return false;
        }
    }

    public class AbilityBase : MonoBehaviour
    {
        public float cooldown;
        public float remainingCooldown;
        public bool useWhileMoving;
        public Character user;
        public AbilityPayload payloadPrefab;
        public AbilityPayload currentPayload;
        public bool inUse;

        public float range;

        public virtual float GetRange()
        {
            return range/100f;
        }
        public virtual bool CanUse(AbilityEventData eventData)
        {
            return remainingCooldown <= 0 && (!user.IsMoving() || useWhileMoving);
        }

        public virtual bool CanReach(AbilityEventData eventData)
        {
            return eventData.TryGetPosition(out var pos) && Vector3.Distance(pos, user.transform.position) <= GetRange();
        }

        public virtual float GetCooldown()
        {
            return cooldown;
        }

        protected virtual void Update()
        {
            if (remainingCooldown > 0)
                remainingCooldown -= Time.deltaTime;
        }

        public virtual void Cancel()
        {
        }

        public virtual void Use(AbilityEventData eventData)
        {
            currentPayload = Instantiate(payloadPrefab);
            user.SetAttack();
            currentPayload.Init(eventData, this, 1f);
            remainingCooldown = GetCooldown();
        }
    }
}