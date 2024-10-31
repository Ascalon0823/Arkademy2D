using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Motor : MonoBehaviour
    {
        public float moveSpeed;
        public Vector2 movDir;
        public bool rotateToMovDir;

        public virtual void FixedUpdate()
        {
            transform.position += (Vector3)(Time.fixedDeltaTime * moveSpeed * movDir.normalized);
            if (rotateToMovDir)
            {
                transform.up = movDir.normalized;
            }
        }
    }
}