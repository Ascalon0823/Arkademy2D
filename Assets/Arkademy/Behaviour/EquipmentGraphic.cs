using UnityEngine;

namespace Arkademy.Behaviour
{
    public class EquipmentGraphic : MonoBehaviour
    {
        
        public Equipment equipment;
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public bool rotateToFace;
        public bool flipToFace;
        
        private void LateUpdate()
        {
            if (rotateToFace)
            {
                transform.up = equipment.facingDir;
            }
            else if (flipToFace)
            {
                transform.up = Vector2.Dot(equipment.facingDir, Vector2.left) >= 0 ? Vector2.left : Vector2.right;
            }
        }
    }
}