using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common;
using Arkademy2D.Common.Objects;
using Arkademy2D.Title.UI;
using UnityEngine;

namespace Arkademy2D.Title.Behaviour
{
    public class TitleMenu : MonoBehaviour
    {
        public GameObject landingCover;
        public PlayerCreationPage playerCreationPage;

        private void Start()
        {
            landingCover.SetActive(true);
        }

        private async void Update()
        {
            if (!landingCover.activeSelf) return;
            if (Input.anyKeyDown)
            {
                await OnStartGame();
            }
        }


        public async Task OnStartGame()
        {
            landingCover.SetActive(false);
            var existingPlayerData = await PlayerSession.Curr.GetPlayerDataAsync(CancellationToken.None);
            if (existingPlayerData is not null)
            {
                Debug.Log($"Player {existingPlayerData.DisplayName} loaded");
                PopulateCharacterList(existingPlayerData);
                return;
            }

            playerCreationPage.BeginCreatePlayer(PopulateCharacterList);
        }

        public void PopulateCharacterList(PlayerData playerData)
        {
            Debug.Log($"Populate character list for {playerData.DisplayName}");
        }
    }
}