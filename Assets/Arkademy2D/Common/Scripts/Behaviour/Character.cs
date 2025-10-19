using System;
using Mirror;
using UnityEngine;

namespace Arkademy2D.Common.Behaviour
{
    public class Character : NetworkBehaviour
    {
        public Vector2 moveDir;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;

        private void Update()
        {
            if (!isOwned) return;
            animator.SetBool("walking", moveDir != Vector2.zero);
        }

        private void FixedUpdate()
        {
            if(!isOwned) return;
            body.MovePosition(body.position + 4f * Time.deltaTime * moveDir);
            
        }
    }
}