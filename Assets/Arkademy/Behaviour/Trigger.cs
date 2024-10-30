using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Behaviour
{
    public class Trigger : MonoBehaviour
    {
        public LayerMask effectiveLayers;

        public UnityEvent<Collider2D> OnEnter;
        public UnityEvent<Collider2D> OnExit;
        public UnityEvent<Collider2D> OnStay;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((effectiveLayers & 1 << other.gameObject.layer) != 1 << other.gameObject.layer) return;
            OnEnter?.Invoke(other);
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if ((effectiveLayers & 1 << other.gameObject.layer) != 1 << other.gameObject.layer) return;
            OnStay?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((effectiveLayers & 1 << other.gameObject.layer) != 1 << other.gameObject.layer) return;
            OnExit?.Invoke(other);
        }
        
    }
}