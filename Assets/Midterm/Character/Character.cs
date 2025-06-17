using System;
using UnityEngine;

namespace Midterm.Character
{
    public class Character : MonoBehaviour
    {
        public Rigidbody2D body;
        public Vector2 moveDir;
        public Vector2 faceDir;
        public float moveSpeed;

        private void Update()
        {
            UpdateGraphic();    
        }

        private void FixedUpdate()
        {
           Move();
        }


        public void Move()
        {
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
            animator.SetBool("walking",isMoving);
            animator.SetFloat("walkSpeed", moveSpeed/4f);
            spriteRenderer.flipX = faceDir.x > 0;
        }
    }
}