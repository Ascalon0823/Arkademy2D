using Arkademy.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI
{
    public class SystemPage : Page
    {
        public async void OnSave()
        {
            await Session.Save();
        }

        public async void OnReturnToTitle()
        {
            await Session.Save();
            SceneManager.LoadScene("Title");
        }
    }
}