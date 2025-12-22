using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Arkademy2D.Game.Behaviours
{
    public class InputHandler : MonoBehaviour
    {
        public Vector2 move;
        public bool cast;
        public bool use;
        public PlayerInput playerInput;
        public Player player;
        private void Awake()
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        private void Update()
        {
            UseActorUsable();
            MoveActor();
        }

        private void MoveActor()
        {
            player.character.movement.moveDir = move.sqrMagnitude > float.Epsilon ? move.normalized : Vector2.zero;
        }

        private void UseActorUsable()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!use) return;
            if (player.character.items?.Count < player.selectedHotbarIdx) return;
            var usableItem = player.character.items[player.selectedHotbarIdx];
            player.character.user.UseItem(usableItem,new UseContext
            {
                character = player.character,
                userTransform = player.character.transform,
            });
        }

        private void Interact()
        {
            if (!player.character.interactionDetector.candidate) return;
            player.character.interactionDetector.candidate.Interact();
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

        public void OnFire(InputValue inputValue)
        {
            use = inputValue.isPressed;
        }
    }
}