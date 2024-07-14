using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionAction<TComponent> : MonoBehaviour where TComponent : MonoBehaviour
    {
        [SerializeField] protected Collider2D c;
        protected Dictionary<TComponent, float> _contactTime = new();

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            var otherComponent = other.gameObject.GetComponent<TComponent>();
            if (!otherComponent) return;
            if (other.gameObject.layer == gameObject.layer) return;
            _contactTime[otherComponent] = 0f;
        }

        protected virtual void OnCollisionExit2D(Collision2D other)
        {
            var otherComponent = other.gameObject.GetComponent<TComponent>();
            if (!otherComponent) return;
            if (!_contactTime.ContainsKey(otherComponent)) return;
            _contactTime.Remove(otherComponent);
        }

        protected virtual void FixedUpdate()
        {
            if (PlayerBehaviour.Player.paused) return;
            var keys = _contactTime.Keys.ToList();
            foreach (var contact in keys)
            {
                ContactUpdated(contact);
            }
        }

        protected virtual void ContactUpdated(TComponent other)
        {
            _contactTime[other] = Time.fixedTime;
        }
    }
}