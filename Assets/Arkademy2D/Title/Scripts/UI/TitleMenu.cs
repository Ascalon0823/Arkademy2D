using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common;
using Arkademy2D.Common.Objects;
using Arkademy2D.Title.Scripts.UI;
using Arkademy2D.Title.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Title.Behaviour
{
    public class TitleMenu : MonoBehaviour
    {
        [SerializeField] private GameObject landingCover;
        [SerializeField] private PlayerCreationPage playerCreationPage;
        [SerializeField] private CharacterCreationPage characterCreationPage;
        [SerializeField] private RectTransform characterSelectionHolder;
        [SerializeField] private CharacterSelectionItem characterSelectionItemPrefab;
        [SerializeField] private Button createCharacterButton;

        private readonly List<CharacterSelectionItem> _populatedCharacterSelectionItems =
            new List<CharacterSelectionItem>();

        [SerializeField] private CharacterSelectionItem currentSelectedItem;
        [SerializeField] private Button startButton;

        private void Start()
        {
            landingCover.SetActive(true);
            characterSelectionHolder.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
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
            currentSelectedItem = null;
            foreach (var item in _populatedCharacterSelectionItems)
            {
                Destroy(item.gameObject);
            }

            startButton.interactable = false;
            _populatedCharacterSelectionItems.Clear();
            foreach (var characterData in playerData.Characters.OrderByDescending(x => x.LastUpdateTime))
            {
                var item = Instantiate(characterSelectionItemPrefab, characterSelectionHolder);
                item.Setup(characterData, SelectCharacterItem);
                _populatedCharacterSelectionItems.Add(item);
                if (!currentSelectedItem)
                {
                    SelectCharacterItem(item);
                }
            }

            characterSelectionHolder.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            createCharacterButton.onClick.RemoveAllListeners();
            createCharacterButton.onClick.AddListener(() => CreateCharacter(playerData));
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartWithSelectedCharacter);
        }

        public void CreateCharacter(PlayerData playerData)
        {
            characterSelectionHolder.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            characterCreationPage.BeginCreateCharacter(playerData,
                newCharacter => { PopulateCharacterList(playerData); });
        }

        public void SelectCharacterItem(CharacterSelectionItem characterSelectionItem)
        {
            if (currentSelectedItem)
            {
                currentSelectedItem.SetSelected(false);
            }

            currentSelectedItem = characterSelectionItem;
            currentSelectedItem.SetSelected(true);
            startButton.interactable = characterSelectionItem;
        }

        public void StartWithSelectedCharacter()
        {
            currentSelectedItem.characterData.LastUpdateTime = DateTime.UtcNow;
            PlayerSession.Curr.LocalCharacterData = currentSelectedItem.characterData;
            Debug.Log($"Starting game with {PlayerSession.Curr.LocalCharacterData.DisplayName}");
        }
    }
}