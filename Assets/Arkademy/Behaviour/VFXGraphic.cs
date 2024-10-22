using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class VFXGraphic : MonoBehaviour
    {
        public Animator animator;
        public float playSpeed;

        private void OnEnable()
        {
            animator.SetFloat("speed", Mathf.Max(0.1f, 1f / playSpeed));
        }
    }
}