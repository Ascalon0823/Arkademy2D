using System;
using UnityEngine;
using UnityEngine.UI;
namespace Arkademy.UI
{
    public class UISpriteSync : MonoBehaviour
    {
        public Image image;
        public SpriteRenderer spriteRenderer;

        private void LateUpdate()
        {
            if (!image || !spriteRenderer) return;
            image.sprite = spriteRenderer.sprite;
        }
    }
}