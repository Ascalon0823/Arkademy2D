using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
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
        public Data.Ability abilityData;
        public float remainingCooldown;
        public Character user;
        public float remainingUseTime;
        public AbilityPayload currPayload;

        public static AbilityBase CreateAbility(Data.Ability data)
        {
            AbilityBase ability = null;
            if (data.abilityPrefab)
            {
                ability = Instantiate(data.abilityPrefab);
            }
            else
            {
                var go = new GameObject(data.name);
                ability = go.AddComponent<AbilityBase>();
            }
            ability.abilityData = data;
            return ability;
        }
        public virtual float GetRange()
        {
            return abilityData.reach;
        }

        public virtual int GetLevel()
        {
            return user.data.GetAbilityLevel(abilityData.name);
        }

        public virtual bool CanUse(AbilityEventData eventData)
        {
            return !InCooldown() && (!InUse()||abilityData.continuous) && (!user.IsMoving() || abilityData.usableWhileMoving);
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
            return abilityData.cooldown;
        }

        public virtual float GetUseTime()
        {
            return abilityData.useTime;
        }

        protected virtual void Update()
        {
            if (remainingCooldown > 0)
                remainingCooldown -= Time.deltaTime;
            if (remainingUseTime > 0)
                remainingUseTime -= Time.deltaTime;
        }

        public virtual void InitPayload(AbilityEventData eventData)
        {
            currPayload = Instantiate(abilityData.payloadPrefab);
            currPayload.Init(eventData, this, GetUseTime(),  null);
        }

        public virtual void HandleInstantPayload(AbilityEventData eventData)
        {
            remainingCooldown = GetCooldown();
            remainingUseTime = GetUseTime();
            InitPayload(eventData);
        }

        public virtual void HandleContinuousPayload(AbilityEventData eventData, bool canceled)
        {
            if (canceled)
            {
                currPayload.UpdatePayload(eventData, canceled);
                currPayload = null;
                remainingCooldown = GetCooldown();
                remainingUseTime = 0;
                return;
            }

            remainingUseTime = GetUseTime();
            if (!currPayload)
            {
                InitPayload(eventData);
            }
            else
            {
                currPayload.UpdatePayload(eventData, canceled);
            }
        }

        public virtual void Use(AbilityEventData eventData, bool canceled = false)
        {
            user.SetAttack(GetUseTime());
            if (!abilityData.continuous)
            {
                HandleInstantPayload(eventData);
            }
            else
            {
                HandleContinuousPayload(eventData, canceled);
            }
        }

        public virtual void GiveToUser(Character newUser)
        {
            user = newUser;
            user.abilities.Add(this);
            transform.parent = user.transform;
        }

        protected virtual void OnDestroy()
        {
            if (user)
                user.abilities.Remove(this);
        }

        public virtual Character GetPrimaryTarget()
        {
            return GetTargetCandidates(out var candidates) ? candidates.First() : null;
        }

        public virtual bool GetTargetCandidates(out List<Character> characters)
        {
            var searchRange = Mathf.Min(GetRange(), user.Attributes.Get(Attribute.Type.Vision));
            characters = Physics2D.OverlapCircleAll(user.transform.position, searchRange)
                .Select(x => x.GetCharacter(out var c) ? c : null)
                .Where(x => x && x.faction != user.faction && !x.isDead)
                .OrderBy(x => Vector3.Distance(x.transform.position, user.transform.position))
                .ToList();
            return characters.Any();
        }
    }
}