using System;
using System.Threading.Tasks;
using Arkademy2D.Common;
using Arkademy2D.Common.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Title.Scripts.UI
{
    public class CharacterCreationPage : MonoBehaviour
    {
        private Action<CharacterData> _onCharacterCreated;
        private PlayerData _currentPlayerData;
        public TMP_InputField characterNameInput;
        public Button confirmButton;

        private void Awake()
        {
            confirmButton.onClick.AddListener(async () => await OnConfirm());
            gameObject.SetActive(false);
        }

        public void BeginCreateCharacter(PlayerData playerData, Action<CharacterData> onCharacterCreated)
        {
            gameObject.SetActive(true);
            _onCharacterCreated = onCharacterCreated;
            _currentPlayerData = playerData;
        }

        public async Task OnConfirm()
        {
            var newCharacter = new CharacterData
            {
                Guid = Guid.NewGuid(),
                DisplayName = characterNameInput.text,
                CreationTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow
            };
            
            _currentPlayerData.Characters.Add(newCharacter);
            await PlayerSession.Curr.SavePlayerDataAsync(_currentPlayerData);
            _onCharacterCreated?.Invoke(newCharacter);
            _onCharacterCreated = null;
            Debug.Log($"Successfully created character data {newCharacter.DisplayName}");
            gameObject.SetActive(false);
        }
    }
}