using UnityEngine;

namespace Arkademy.Gameplay
{
    public class InteractableIndicator : MonoBehaviour
    {
        public SpriteRenderer graphic;

        private void LateUpdate()
        {
            if (!Player.LocalPlayer) return;
            graphic.gameObject.SetActive(Player.LocalPlayer.currentInteractableCandidate);
            if (Player.LocalPlayer.currentInteractableCandidate)
            {
                graphic.transform.position = Player.LocalPlayer.currentInteractableCandidate.transform.position;
            }
        }
    }
}