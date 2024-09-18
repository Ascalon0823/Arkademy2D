using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : MonoBehaviour
    {
        public bool onEnter;
        public bool onExit;
        public float cooldown;
        public float remainingCooldown;
        public UnityEvent<Collider2D> onTrigger;
        public List<Collider2D> records = new List<Collider2D>();

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (onEnter)
            {
                onTrigger?.Invoke(other);
            }
            if (!cooldown.Equals(0))
            {
                records.Add(other);
            }
        }

        protected void FixedUpdate()
        {
            if (Player.Paused) return;
            remainingCooldown -= Time.fixedDeltaTime;
            if (remainingCooldown >0) return;
            foreach (var c in records)
            {
                if(c)onTrigger?.Invoke(c);
            }

            remainingCooldown = cooldown;
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (onExit)
                onTrigger?.Invoke(other);
            if (!cooldown.Equals(0) && records.Contains(other))
            {
                records.Remove(other);
            }
        }
    }
}