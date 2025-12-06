using System;
using System.Collections;
using Arkademy2D.Core.System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI
{
    public class SavePage : MonoBehaviour
    {
        public static void SetOpen(bool open)
        {
            _instance.gameObject.SetActive(open);
        }
        
        private static SavePage _instance;
        public Button saveButton;

        private void Awake()
        {
            _instance = this;
            SetOpen(false);
        }

        private void OnEnable()
        {
            FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("UI");
            StartCoroutine(SelectButton());
        }

        IEnumerator SelectButton()
        {
            yield return null;
            saveButton.Select();
        }
        private void OnDisable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("Player");
        }

        public void OnSaveClicked()
        {
            SaveSystem.SavePlayer();
            SetOpen(false);
        }
    }
}