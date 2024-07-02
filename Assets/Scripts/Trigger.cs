using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : MonoBehaviour
    {
        public UnityEvent<Collider2D> onTrigger;
        private void OnTriggerEnter2D(Collider2D other)
        {
            onTrigger?.Invoke(other);
        }
    }
}