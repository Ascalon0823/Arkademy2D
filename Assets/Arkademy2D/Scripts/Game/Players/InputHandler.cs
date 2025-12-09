using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Arkademy2D.Game.Player
{
    public class InputHandler : MonoBehaviour
    {
        public Vector2 move;
        public Actor.Movement actorMovement;
        public Interaction.Detector interactionDetector;
        public bool cast;
        private bool OnUI()
        {
            return EventSystem.current.currentSelectedGameObject;
        }

        private void Update()
        {
            //if (OnUI()) return;
            MoveActor();
        }

        private void MoveActor()
        {
            if (!actorMovement) return;
            actorMovement.moveDir = move.sqrMagnitude > float.Epsilon ? move.normalized : Vector2.zero;
        }

        private void Interact()
        {
            if (!interactionDetector) return;
            if (!interactionDetector.candidate) return;
            interactionDetector.candidate.Interact();
        }

        public void OnInteract(InputValue inputValue)
        {
            if (inputValue.isPressed)
            {
                Interact();
            }
        }

        public void OnMove(InputValue inputValue)
        {
            move = inputValue.Get<Vector2>();
        }

        public void OnCast(InputValue inputValue)
        {
            cast = inputValue.isPressed;
        }
    }
}