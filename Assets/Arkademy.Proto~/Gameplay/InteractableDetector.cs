using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class InteractableDetector : MonoBehaviour
    {
        public float radius;
        public CircleCollider2D trigger;
        public List<Collider2D> exclude = new List<Collider2D>();
        public List<Interactable> interactables = new List<Interactable>();

        private void FixedUpdate()
        {
            trigger.radius = radius;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (exclude.Contains(other) || !other.GetInteractable(out var interactable)) return;
            if (interactables.Contains(interactable)) return;
            interactables.Add(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (exclude.Contains(other) || !other.GetInteractable(out var interactable)) return;
            if (!interactables.Contains(interactable)) return;
            interactables.Remove(interactable);
        }
    }
}