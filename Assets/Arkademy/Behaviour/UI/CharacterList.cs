using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class CharacterList : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private CharacterListItem itemPrefab;
        [SerializeField] private CharacterListItem currentSelection;
        private List<CharacterListItem> _spawnedListItems = new List<CharacterListItem>();

        public void SelectItem(CharacterListItem child)
        {
            if (currentSelection)
            {
                currentSelection.Select(false);
            }

            currentSelection = child;
            currentSelection.Select(true);
        }

        public void SetCharacters(List<CharacterRecord> records)
        {
            foreach (var old in _spawnedListItems)
            {
                Destroy(old.gameObject);
            }

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