using System;
using System.Threading.Tasks;
using Arkademy.Backend;
using Arkademy.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arkademy.UserAuth
{
    public class UserAuthPage : MonoBehaviour
    {
        public bool login;
        [SerializeField] private Image loginHighlight;
        [SerializeField] private Image registerHighlight;

        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField passwordInputField;

        [SerializeField] private Button confirmButton;

        private void Awake()
        {
            SetUseLogin(true);
            usernameInputField.onValueChanged.AddListener(s=>UpdateConfirmButton());
            passwordInputField.onValueChanged.AddListener(s=>UpdateConfirmButton());
            UpdateConfirmButton();
        }

        private void UpdateConfirmButton()
        {
            confirmButton.interactable = !string.IsNullOrEmpty(usernameInputField.text)
                                         && !string.IsNullOrEmpty(passwordInputField.text);
        }

        public void SetUseLogin(bool useLogin)
        {
            login = useLogin;
            UpdateLoginRegisterButton();
        }

        private void UpdateLoginRegisterButton()
        {
            loginHighlight.enabled = login;
            registerHighlight.enabled = !login;
        }

        public void Cancel()
        {
            SceneManager.LoadScene("Title");
        }

        private async Task Login(string username, string password)
        {
            if (!await BackendService.Login(username, password))
            {
                return;
            }
            var user = await BackendService.GetUser();
            if (user == null)
            {
                return;
            }

            Session.currPlayerRecord = user.playerRecord.ToPlayerRecordData();
            await Session.Save();
            SceneManager.LoadScene("CharacterSelection");
        }
        public async void Confirm()
        {
            var username = usernameInputField.text;
            var password = passwordInputField.text;
            if (!login)
            {

                if (!await BackendService.Register(username, password))
                {
                    return;
                }
            }
            await Login(username, password);
        }
    }
}