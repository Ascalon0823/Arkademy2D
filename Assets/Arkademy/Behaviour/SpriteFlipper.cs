using UnityEngine;

namespace Arkademy.Behaviour
{
    [ExecuteInEditMode]
    public class SpriteFlipper : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool flipY;
        [SerializeField] private bool flipX;
        [SerializeField] private Vector2 flipYRefAxis;
        [SerializeField] private Vector2 flipYUseAxis;
        [SerializeField] private Vector2 flipXRefAxis;
        [SerializeField] private Vector2 flipXUseAxis;

        private void LateUpdate()
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
    }
}