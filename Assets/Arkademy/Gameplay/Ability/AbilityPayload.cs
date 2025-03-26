using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Gameplay.Ability
{
    public class AbilityPayload : MonoBehaviour
    {
        public AbilityBase ability;
        public float duration;
        public float triggerPoint;
        public UnityEvent<AbilityPayload> OnTriggered;
        public bool triggered;

        public virtual void Init(AbilityEventData data,
            AbilityBase parent, float dura, float triggerTime,
            Action<AbilityPayload> onTriggered)
        {
            ability = parent;
            duration = dura;
            triggerPoint = dura - triggerTime;
            if (onTriggered != null)
            {
                OnTriggered.AddListener(p => onTriggered(p));
            }
            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            var direction = data.TryGetDirection(parent.user.transform.position, out var dir)
                ? dir
                : parent.user.facing;
            transform.up = direction;
        }

        public virtual void UpdatePayload(AbilityEventData data)
        {
        }

        public virtual void Cancel()
        {
        }

        protected virtual void Update()
        {
            duration -= Time.deltaTime;
            if (duration < triggerPoint && !triggered)
            {
                triggered = true;
                Trigger();
            }

            if (duration < 0)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Trigger()
        {
            if (!this) return;
            OnTriggered?.Invoke(this);
        }
    }
}