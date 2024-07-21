using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI.Game
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;

        private void Start()
        {
            menuPanel.SetActive(false);
        }

        public void Toggle(bool active)
        {
            if (active != menuPanel.activeSelf)
            {
                PlayerBehaviour.Player.pauseCount += active ? 1 : -1;
            }
            menuPanel.SetActive(active);
            
        }

        public void Toggle()
        {
            Toggle(!menuPanel.activeSelf);
        }
    }

}
