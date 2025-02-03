using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Gameplay
{
    public class Trigger : MonoBehaviour
    {
        public UnityEvent<Collider2D> OnTrigger;
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTrigger?.Invoke(other);
        }
    }
}