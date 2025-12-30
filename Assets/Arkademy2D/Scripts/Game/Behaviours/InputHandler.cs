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
        public bool wasCasting;
        public bool canCast;
        public bool use;
        public PlayerInput playerInput;
        public Player player;

        private void Awake()
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        private void Update()
        {
            TryCast();
            UseActorUsable();
            MoveActor();
        }

        private void TryCast()
        {
            var caster = player.character.caster;
            if (!caster) return;
            caster.casting = cast;
            if (!cast)
            {
                if (wasCasting)
                {
                    caster.EndCast();
                }

                canCast = false;
                return;
            }

            wasCasting = true;
            var dir = move.normalized;
            if (dir.magnitude < float.Epsilon)
            {
                canCast = true;
                return;
            }

            if (!canCast) return;
            if (Vector2.Dot(dir, Vector2.up) >= 0.5f)
            {
                caster.Cast("W");
            }else if (Vector2.Dot(dir, Vector2.down) >= 0.5f)
            {
                caster.Cast("S");
            }else if (Vector2.Dot(dir, Vector2.left) >= 0.5f)
            {
                caster.Cast("A");
            }else if (Vector2.Dot(dir, Vector2.right) >= 0.5f)
            {
                caster.Cast("D");
            }
        }

        private void MoveActor()
        {
            player.character.movement.moveDir =
                move.sqrMagnitude <= float.Epsilon || cast ? Vector2.zero : move.normalized;
        }

        private void UseActorUsable()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!use || cast) return;
            if (player.character.items?.Count < player.selectedHotbarIdx) return;
            // var usableItem = player.character.items[player.selectedHotbarIdx];
            // player.character.UseItem(usableItem, new UseContext
            // {
            //     character = player.character,
            //     userTransform = player.character.transform,
            // });
        }

        private void Interact()
        {
            if (cast) return;
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