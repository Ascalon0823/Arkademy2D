using UnityEngine;
using Arkademy2D.Game.Interaction;
namespace Arkademy2D.Game.Behaviours
{
    public class InteractionIndicator : MonoBehaviour
    {
        public Character character;
        public GameObject indicator;

        private void LateUpdate()
        {
            indicator.SetActive(character.interactionCandidate);
            if (!character.interactionCandidate) return;
            indicator.transform.position = character.interactionCandidate.transform.position;
        }
    }
}