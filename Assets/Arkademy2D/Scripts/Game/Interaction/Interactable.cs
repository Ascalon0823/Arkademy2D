using UnityEngine;
using UnityEngine.Events;

namespace Arkademy2D.Game.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent onInteracted;
        public virtual void Interact()
        {
            onInteracted?.Invoke();
            Debug.Log("Interact", this);
        }
    }
}