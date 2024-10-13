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
        [SerializeField] private Button cancelButton;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(s => { confirmButton.interactable = !string.IsNullOrEmpty(s); });
            confirmButton.interactable = false;
            gameObject.SetActive(false);
        }

        public void Activate(Action onCancel = null, Action<PlayerRecord> onComplete = null)
        {
            gameObject.SetActive(true);

            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke(new PlayerRecord
                {
                    playerName = inputField.text,
                    CreationTime = DateTime.UtcNow,
                    LastPlayed = DateTime.UtcNow
                });
            });

            cancelButton.onClick?.AddListener(() =>
            {
                gameObject.SetActive(false);
                onCancel?.Invoke();
            });
        }
    }
}