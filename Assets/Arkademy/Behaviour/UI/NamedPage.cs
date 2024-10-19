using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class NamedPage : MonoBehaviour
    {
        public string key;
        public string displayName;

        [SerializeField] protected CanvasGroup cg;
        public void Activate(string currKey)
        {
            var activate = key == currKey;
            cg.interactable = activate;
            cg.blocksRaycasts = activate;
            cg.alpha = activate ? 1 : 0;
        }
    }
}