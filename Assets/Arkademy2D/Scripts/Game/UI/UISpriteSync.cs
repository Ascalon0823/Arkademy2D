using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI
{
    public class UISpriteSync : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Image image;

        [ExecuteInEditMode]
        private void LateUpdate()
        {
            image.sprite = spriteRenderer.sprite;
        }
    }
}
