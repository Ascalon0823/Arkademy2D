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
        public float walkSpeed;
        public float attackSpeed;
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int Attack = Animator.StringToHash("attack");

        private void LateUpdate()
        {
            if (facing.sqrMagnitude > 0f)
            {
                spriteRenderer.flipX = Vector2.Dot(facing, Vector2.left) >= 0 ? !facingLeft : facingLeft;
            }

            animator.SetFloat("walkSpeed", walkSpeed);
            animator.SetFloat("attackSpeed", attackSpeed);
            animator.SetBool(Walking, moveDir.sqrMagnitude > 0f);
        }

        public void SetAttack()
        {
            animator.SetTrigger(Attack);
        }
    }
}