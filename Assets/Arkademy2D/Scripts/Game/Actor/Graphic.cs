using UnityEngine;

namespace Arkademy2D.Game.Actor
{
    public class Graphic : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public Animator animator;

        public Movement movement;
        public Health health;

        public bool spriteFaceLeft;

        public void SetAnimationTrigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
        private void LateUpdate()
        {
            if (health)
            {
                animator.SetBool("dead",health.current==0);
            }
            if (movement)
            {
                animator.SetBool("walking", movement.moveDir.sqrMagnitude > float.Epsilon && health.current >0);
                sprite.flipX = Vector2.Dot(movement.faceDir, Vector2.right) > 0 ? spriteFaceLeft : !spriteFaceLeft;
            }
        }
    }
}