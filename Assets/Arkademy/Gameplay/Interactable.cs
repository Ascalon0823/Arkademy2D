using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Interactable : MonoBehaviour
    {
        public string interactableInfo;
        public float radius;
        [SerializeField] private CircleCollider2D trigger;

        private void OnEnable()
        {
            trigger.RegisterInteractableTrigger(this);
        }

        private void FixedUpdate()
        {
            trigger.radius = radius;
        }

        public virtual bool OnInteractedBy(Character character)
        {
            Debug.Log($"Interacted by {character.name}");
            return true;
        }

        public virtual string GetPopup(Character character)
        {
            return interactableInfo;
        }
    }
}