using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class PlayerRecordCreation : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(s => { confirmButton.interactable = !string.IsNullOrEmpty(s); });
            confirmButton.interactable = false;
            gameObject.SetActive(false);
        }

        public void Confirm()
        {
            var newRecord = new PlayerRecord
            {
                playerName = inputField.text,
                CreationTime = DateTime.UtcNow,
                LastPlayed = DateTime.UtcNow
            };
            MainMenu.AddPlayerRecord(newRecord);
            MainMenu.AddSelectedPlayerRecord(newRecord);
            gameObject.SetActive(false);
        }
    }
}