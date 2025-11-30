using System;
using UnityEngine;

namespace Arkademy2D.Game.Actors
{
    public class Graphic : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public Animator animator;

        public Movement movement;

        public bool spriteFaceLeft;

        private void LateUpdate()
        {
            animator.SetBool("walking", movement.moveDir.sqrMagnitude > float.Epsilon);
            sprite.flipX = Vector2.Dot(movement.moveDir, Vector2.right) > 0 ? spriteFaceLeft : !spriteFaceLeft;
        }
    }
}