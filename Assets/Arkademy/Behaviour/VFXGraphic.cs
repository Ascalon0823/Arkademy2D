using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    [ExecuteInEditMode]
    public class VFXGraphic : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public float playSpeed;
        public bool flipY;
        public bool flipX;
        public Vector2 flipYRefAxis;
        public Vector2 flipYUseAxis;
        public Vector2 flipXRefAxis;
        public Vector2 flipXUseAxis;

        private void Update()
        {
            if (flipY)
            {
                spriteRenderer.flipY =
                    Vector2.Dot(transform.TransformDirection(flipYUseAxis), flipYRefAxis.normalized) < 0;
            }

            if (flipX)
            {
                spriteRenderer.flipX =
                    Vector2.Dot(transform.TransformDirection(flipXUseAxis), flipXRefAxis.normalized) < 0;
            }
        }

        private void OnEnable()
        {
            animator.SetFloat("speed", Mathf.Max(0.1f, 1f / playSpeed));
        }
    }
}