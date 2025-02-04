using UnityEngine;
using UnityEngine.SceneManagement;
using Arkademy.Data;

namespace Arkademy.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public CanvasGroup pauseCanvasGroup;

        public bool activated;
        protected void Awake()
        {
            Toggle(false);
        }

        public void Toggle()
        {
            Toggle(!activated);
        }

        public void Toggle(bool active)
        {
            activated = active;
            pauseCanvasGroup.interactable = active;
            pauseCanvasGroup.blocksRaycasts = active;
            pauseCanvasGroup.alpha = active ? 1f : 0f;
        }

        public void OnResume()
        {
            Toggle(false);
        }
        public void OnSave()
        {
            Session.Save();
        }

        public void OnReturnToTitle()
        {
            Session.Save();
            SceneManager.LoadScene("Title");
        }
    }
}