using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class CharacterList : MonoBehaviour
    {
        [SerializeField] private PlayerRecord currentPlayerRecord;
        [SerializeField] private RectTransform container;
        [SerializeField] private CharacterListItem itemPrefab;
        [SerializeField] private CharacterListItem currentSelection;
        [SerializeField] private CharacterCreation characterCreation;
        private List<CharacterListItem> _spawnedListItems = new List<CharacterListItem>();

        [SerializeField] private Button cancel;
        [SerializeField] private Button confirm;


        public void SelectItem(CharacterListItem child)
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

        public void BeginCharacterCreation()
        {
            characterCreation.Activate(c =>
            {
                MainMenu.AddCharacterRecord(new CharacterRecord
                {
                    characterData = c,
                    CreationTime = DateTime.UtcNow,
                    LastPlayed = DateTime.UtcNow
                }, currentPlayerRecord);
                SetCharacters(currentPlayerRecord.characterRecords);
            });
        }

        public void Activate(List<CharacterRecord> records, PlayerRecord playerRecord, Action onCancel = null,
            Action<CharacterRecord> onConfirm = null)
        {
            currentPlayerRecord = playerRecord;
            gameObject.SetActive(true);
            cancel.onClick.RemoveAllListeners();
            confirm.onClick.RemoveAllListeners();
            confirm.interactable = false;
            SetCharacters(records);

            cancel.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onCancel?.Invoke();
            });

            confirm.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onConfirm?.Invoke(currentSelection.record);
            });
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
                item.list = this;
            }

            if (_spawnedListItems.Count > 0)
            {
                SelectItem(_spawnedListItems[0]);
            }

            var addSign = Instantiate(itemPrefab, container);
            addSign.list = this;
            _spawnedListItems.Add(addSign);
        }
    }
}