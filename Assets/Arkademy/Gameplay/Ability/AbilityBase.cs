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
        public float useTime;
        public float remainingUseTime;
        public float range;

        public virtual float GetRange()
        {
            return range / 100f;
        }

        public virtual bool CanUse(AbilityEventData eventData)
        {
            return !InCooldown() && !InUse() && (!user.IsMoving() || useWhileMoving);
        }

        public virtual bool InCooldown()
        {
            return remainingCooldown > 0;
        }

        public virtual bool InUse()
        {
            return remainingUseTime > 0;
        }

        public virtual bool CanReach(AbilityEventData eventData)
        {
            return eventData.TryGetPosition(out var pos) &&
                   Vector3.Distance(pos, user.transform.position) <= GetRange();
        }

        public virtual float GetCooldown()
        {
            return cooldown;
        }

        public virtual float GetUseTime()
        {
            return useTime;
        }

        protected virtual void Update()
        {
            if (remainingCooldown > 0)
                remainingCooldown -= Time.deltaTime;
            if (remainingUseTime > 0)
                remainingUseTime -= Time.deltaTime;
        }

        public virtual void Use(AbilityEventData eventData, bool canceled = false)
        {
            user.SetAttack(GetUseTime());
            remainingUseTime = GetUseTime();
            remainingCooldown = GetCooldown();
        }

        public virtual void GiveToUser(Character newUser)
        {
            user = newUser;
            user.abilities.Add(this);
        }

        protected virtual void OnDestroy()
        {
            if(user)
                user.abilities.Remove(this);
        }
    }
}