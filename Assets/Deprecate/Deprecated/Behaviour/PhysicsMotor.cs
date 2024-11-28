using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsMotor : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float speed;
        public Vector2 moveDir;
        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + Time.fixedDeltaTime * speed * moveDir.normalized);
        }
    }
}