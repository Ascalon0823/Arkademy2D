using System;
using UnityEngine;

namespace Arkademy2D.Game.Player
{
    public class InputHandler : MonoBehaviour
    {
        public Vector2 move;
        public Actors.Movement actorMovement;
        private void Update()
        {
            MoveActor();
        }

        private void MoveActor()
        {
            if (!actorMovement) return;
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");
            
            move = move.sqrMagnitude > float.Epsilon ? move.normalized : Vector2.zero;
            actorMovement.moveDir = move;
        }
    }
}