using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Arkademy.Rift;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arkademy.Campus.UI
{
    public class RiftStartMenu : MonoBehaviour
    {
        private static RiftStartMenu _instance;

        [SerializeField] private int currDifficulty;
        [SerializeField] private Button difficultySelectionPrefab;
        [SerializeField] private RectTransform content;
        private List<Button> _spawnedButtons = new List<Button>();
        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            Toggle(false);
        }

        public static void Show()
        {
            _instance.Toggle(true);
        }

        public void Toggle(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                SpawnButtons();
            }
        }

        public void Confirm()
        {
            RiftController.RiftSetup = currDifficulty;
            SceneManager.LoadScene("Rift");
        }

        public void SpawnButtons()
        {
            foreach (var b in _spawnedButtons)
            {
                Destroy(b.gameObject);
            }
            _spawnedButtons.Clear();
            var maxButton = Session.currCharacterRecord.character.clearedRift + 1;
            for (var i = maxButton; i >= 0; i--)
            {
                var level = i;
                var b = Instantiate(difficultySelectionPrefab, content);
                b.onClick.AddListener(() =>
                {
                    currDifficulty = level;
                });
                b.GetComponentInChildren<TextMeshProUGUI>().text = $"Difficulty: {i}";
                _spawnedButtons.Add(b);
            }
            _spawnedButtons.First().Select();
            currDifficulty = maxButton;
        }
    }
}