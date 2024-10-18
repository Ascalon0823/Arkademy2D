using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.Behaviour.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        public void Toggle(bool active)
        {
            menuPanel.SetActive(active);
            Game.RequirePause(active);
        }

        public void Toggle()
        {
            Toggle(!menuPanel.activeInHierarchy);
        }

        private void Start()
        {
            Toggle(false);
        }

        public void ReturnToMainMenu()
        {
            Game.localPlayers.ForEach(x=>x.playerRecord.Save());
            SceneManager.LoadScene("Arkademy/Scenes/MainMenu");
        }
    }
}