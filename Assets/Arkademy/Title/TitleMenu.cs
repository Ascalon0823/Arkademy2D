using Arkademy.Backend;
using Arkademy.Data;
using Newtonsoft.Json;
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

        public async void BeginSession()
        {
            var user = await BackendService.GetUser();
            if (user == null)
            {
                SceneManager.LoadScene("UserAuth");
                return;
            }
            Session.currPlayerRecord = user.playerRecord.ToPlayerRecordData();
            Debug.Log(JsonConvert.SerializeObject(user));
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
        [ContextMenu("Clear User Cache")]
        public void ClearUserCache()
        {
            PlayerPrefs.DeleteKey("PlayerToken");
        }
    }
}