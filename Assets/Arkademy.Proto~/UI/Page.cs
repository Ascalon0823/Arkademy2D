using UnityEngine;

namespace Arkademy.UI
{
    public class Page : MonoBehaviour
    {
        public CanvasGroup group;

        public virtual void Toggle(bool on)
        {
            group.alpha = on ? 1 : 0;
            group.interactable = on;
            group.blocksRaycasts = on;
            if(on)OnShow();
        }

        public virtual void OnShow()
        {
            
        }
    }
}