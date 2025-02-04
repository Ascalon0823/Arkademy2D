using System;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay
{
    public class CharacterGraphic : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public bool facingLeft;
        public Animator animator;
        public GameObject shadow;
        public float walkAnimationDistance;
        public float attackSpeed;
        public Gameplay.Character character;
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int Attack = Animator.StringToHash("attack");

        private void LateUpdate()
        {
            if (character.facing.sqrMagnitude > 0f && !character.isDead)
            {
                spriteRenderer.flipX = Vector2.Dot(character.facing, Vector2.left) >= 0 ? !facingLeft : facingLeft;
            }

            animator.SetFloat("walkSpeed", character.data.Get(Attribute.Type.MovSpeed) / walkAnimationDistance);
            animator.SetFloat("attackSpeed", attackSpeed);
            animator.SetBool(Walking, character.IsMoving() && !character.isDead);
            animator.SetBool("dead", character.isDead);
        }

        public void SetAttack()
        {
            animator.SetTrigger(Attack);
        }

        public void SetHit()
        {
            animator.SetTrigger("hit");
        }
    }
}