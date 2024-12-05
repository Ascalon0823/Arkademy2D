using System;
using Arkademy.Behaviour;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.Title
{
    public class TitleMenu : MonoBehaviour
    {
        [SerializeField] private GameObject landingPage;
        [SerializeField] private GameObject buttons;

        [SerializeField] private GameObject debugPage;
        [SerializeField] private TextMeshProUGUI versionText;
        private void Awake()
        {
            landingPage.SetActive(true);
            buttons.SetActive(false);
            debugPage.SetActive(false);
            versionText.text = Application.version;
        }

        public void BeginSession()
        {
            landingPage.SetActive(false);
            buttons.SetActive(true);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("CharacterSelection");
        }

        public void ToggleDebugPage()
        {
            debugPage.SetActive(!debugPage.activeSelf);
        }

        public void TestInputScene()
        {
            SceneManager.LoadScene("TestInput");
        }

        public void ClearSaveData()
        {
            PlayerPrefs.DeleteKey("PlayerData");
        }
    }
}