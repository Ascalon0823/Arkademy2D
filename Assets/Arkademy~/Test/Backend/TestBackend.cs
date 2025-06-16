using System;
using System.Threading.Tasks;
using Arkademy.Backend;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Test.Backend
{
    public class TestBackend : MonoBehaviour
    {
        public string userName;
        public string password;
        [TextArea]
        public string userSerialized;
        public User user;
        private async void Start()
        {
            await GetUser();
            if (user != null) return;
            await Login();
            await GetUser();
        }

        [ContextMenu("Clear Token")]
        public void ClearLocalToken()
        {
            PlayerPrefs.DeleteKey("PlayerToken");
        }
        public async Task GetUser()
        {
            user = await BackendService.GetUser();
            userSerialized = JsonConvert.SerializeObject(user,Formatting.Indented);
        }
        public async Task Login()
        {
            await BackendService.Login(userName, password);
        }
    }
}