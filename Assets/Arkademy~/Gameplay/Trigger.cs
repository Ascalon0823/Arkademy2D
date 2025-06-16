using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Gameplay
{
    public class Trigger : MonoBehaviour
    {
        public Collider2D trigger;
        public UnityEvent<Collider2D> OnTrigger;
        public HashSet<Collider2D> Ignores = new HashSet<Collider2D>();
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Ignores.Contains(other)) return;
            OnTrigger?.Invoke(other);
        }
    }
}