using System;
using System.Collections.Generic;
using UnityEngine;

namespace Midterm.Character
{
    public class Character : MonoBehaviour
    {
        public Rigidbody2D body;
        public new CircleCollider2D collider;
        public Vector2 moveDir;
        public Vector2 faceDir;
        public float moveSpeed;

        private void Update()
        {
            UseAbilities();
        }

        private void LateUpdate()
        {
            UpdateGraphic();
        }

        private void FixedUpdate()
        {
            collider.enabled = life > 0;
            Move();
        }


        public void Move()
        {
            if (life <= 0)
            {
                return;
            }

            body.MovePosition(body.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
            if (moveDir.magnitude > 0.01f)
            {
                faceDir = moveDir.normalized;
            }
        }

        public Animator animator;
        public SpriteRenderer spriteRenderer;

        public void UpdateGraphic()
        {
            var isMoving = moveDir.magnitude > 0.01f;
            animator.SetBool("walking", isMoving);
            animator.SetFloat("walkSpeed", moveSpeed / 4f);
            spriteRenderer.flipX = faceDir.x > 0;
            animator.SetBool("dead", life <= 0);
        }

        public int life;
        public int maxLife;

        public void TakeDamage(int damage)
        {
            life -= damage;
            life = Mathf.Clamp(life, 0, maxLife);
            animator.SetTrigger("hit");
        }
        
        public List<Ability> abilities;

        public void UseAbilities()
        {
            foreach (var ability in abilities)
            {
                if (ability.CanUse())
                {
                    ability.Use();  
                }
            }
        }
    }
}