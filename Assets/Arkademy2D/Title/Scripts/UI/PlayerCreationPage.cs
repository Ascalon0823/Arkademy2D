using System;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common;
using Arkademy2D.Common.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Title.UI
{
    public class PlayerCreationPage : MonoBehaviour
    {
        public TMP_InputField playerNameInput;
        public Button confirmButton;
        public Action<PlayerData> OnPlayerDataCreated;

        private void Awake()
        {
            confirmButton.onClick.AddListener(async () => await OnConfirm());
            gameObject.SetActive(false);
        }

        public void BeginCreatePlayer(Action<PlayerData> onPlayerDataCreated)
        {
            gameObject.SetActive(true);
            OnPlayerDataCreated = onPlayerDataCreated;
        }

        public async Task OnConfirm()
        {
            var createdPlayerData =
                await PlayerSession.Curr.CreatePlayerDataAsync(playerNameInput.text, CancellationToken.None);
            if (createdPlayerData is null)
            {
                Debug.LogWarning($"Failed to create player data");
                return;
            }

            Debug.Log($"Successfully created player data {createdPlayerData.DisplayName}");
            gameObject.SetActive(false);
            OnPlayerDataCreated?.Invoke(createdPlayerData);
        }
    }
}