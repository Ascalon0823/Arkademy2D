using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class CharacterGraphic : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public bool facingLeft;
        public Animator animator;
        public GameObject shadow;
        public Vector2 facing;
        public Vector2 moveDir;
        public float walkAnimationDistance;
        public float walkSpeed;
        public float attackSpeed;
        public Gameplay.Character character;
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int Attack = Animator.StringToHash("attack");

        private void LateUpdate()
        {
            moveDir = character.move;
            walkSpeed = character.GetMoveSpeed();

            if (moveDir.magnitude > 0f)
            {
                facing = moveDir;
            }

            if (facing.sqrMagnitude > 0f)
            {
                spriteRenderer.flipX = Vector2.Dot(facing, Vector2.left) >= 0 ? !facingLeft : facingLeft;
            }

            animator.SetFloat("walkSpeed", walkSpeed / walkAnimationDistance);
            animator.SetFloat("attackSpeed", attackSpeed);
            animator.SetBool(Walking, moveDir.sqrMagnitude > 0f);
        }

        public void SetAttack()
        {
            animator.SetTrigger(Attack);
        }

        public void SetDead(bool isDead = true)
        {
            animator.SetBool("dead", isDead);
        }

        public void SetHit()
        {
            animator.SetTrigger("hit");
        }
    }
}