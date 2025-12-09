using System;
using UnityEngine;

namespace Arkademy2D.Game.Actor
{
    public class Graphic : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public Animator animator;

        public Movement movement;

        public bool spriteFaceLeft;

        public void SetAnimationTrigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
        private void LateUpdate()
        {
            animator.SetBool("walking", movement.moveDir.sqrMagnitude > float.Epsilon);
            sprite.flipX = Vector2.Dot(movement.faceDir, Vector2.right) > 0 ? spriteFaceLeft : !spriteFaceLeft;
        }
    }
}