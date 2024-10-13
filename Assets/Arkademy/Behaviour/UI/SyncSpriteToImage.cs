using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SyncSpriteToImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer; 
    [SerializeField] Image imageComponent;         

    void LateUpdate()
    {
        // Ensure both references are assigned
        if (spriteRenderer && imageComponent )
        {
            // Update the Image's sprite with the SpriteRenderer's sprite
            imageComponent.sprite = spriteRenderer.sprite;
        }
    }
}