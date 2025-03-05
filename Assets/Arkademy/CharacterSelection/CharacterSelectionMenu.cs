using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Backend;
using Arkademy.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CharacterRecord = Arkademy.Data.CharacterRecord;

namespace Arkademy.CharacterSelection
{
    public class CharacterSelectionMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private CharacterSelectionItem itemPrefab;
        [SerializeField] private CharacterSelectionItem currentSelection;
        private List<CharacterSelectionItem> _spawnedListItems = new List<CharacterSelectionItem>();

        [SerializeField] private Button cancel;
        [SerializeField] private Button confirm;

        private void Awake()
        {
            confirm.interactable = false;
            cancel.onClick.AddListener(OnCancel);
            var characters = Session.currPlayerRecord.characterRecords;
            SetCharacters(characters.OrderByDescending(x=>x.LastPlayed).ToList());
            
        }

        public void OnCancel()
        {
            SceneManager.LoadScene("Arkademy/Title/Title");
        }
        public async void OnConfirm()
        {
            Session.currCharacterRecord = currentSelection.record;
            currentSelection.record.LastPlayed = DateTime.UtcNow;
            await Session.Save();
            SceneManager.LoadScene("Campus");
        }

        public void SelectItem(CharacterSelectionItem child)
        {
            if (currentSelection)
            {
                currentSelection.Select(false);
            }

            if (!child || child.record == null) return;
            confirm.interactable = true;
            currentSelection = child;
            currentSelection.Select(true);
        }
        

        private void SetCharacters(List<CharacterRecord> records)
        {
            foreach (var old in _spawnedListItems)
            {
                Destroy(old.gameObject);
            }

            _spawnedListItems.Clear();

            currentSelection = null;

            foreach (var record in records)
            {
                var item = Instantiate(itemPrefab, container);
                item.Setup(record);
                _spawnedListItems.Add(item);
                item.menu = this;
            }

            if (_spawnedListItems.Count > 0)
            {
                SelectItem(_spawnedListItems[0]);
            }

            var addSign = Instantiate(itemPrefab, container);
            addSign.menu = this;
            _spawnedListItems.Add(addSign);
        }
    }
}