using System;
using UnityEngine;

namespace Arkademy
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CircleCollider2D collision;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Vector2 moveDir;
        public float moveSpeed;
        public Vector2 pointAt;


        public Usable currUsable;

        public int life;
        public int maxLife;
        private void FixedUpdate()
        {
            if (life == 0) return;
            body.MovePosition(body.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            var walking = moveDir.magnitude > 0.01f;
            animator.SetBool("walking", walking);
            var faceDir = pointAt - body.position;
            spriteRenderer.flipX = faceDir.x > 0;
        }

        public void Use()
        {
            if (life == 0) return;
            if (!currUsable) return;
            if (!currUsable.CanUse(pointAt)) return;
            currUsable.Use(pointAt);
            animator.SetFloat("attackSpeed",1/currUsable.remainingUseTime);
            animator.SetTrigger("attack");
        }
    }
}