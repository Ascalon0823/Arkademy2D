using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI.Title
{
    public class CharacterSelectionPage : MonoBehaviour
    {
        [SerializeField] private RectTransform contentList;
        [SerializeField] private CharacterSelectionListItem itemPrefab;

        [SerializeField]
        private List<CharacterSelectionListItem> generatedItems = new List<CharacterSelectionListItem>();

        [SerializeField] private int currentSelectionIdx;

        private void OnEnable()
        {
            foreach (var item in generatedItems)
            {
                Destroy(item.gameObject);
            }

            generatedItems.Clear();

            var playableCharacterData = Database.GetDatabase().playableCharacterData;
            for (var i = 0; i < playableCharacterData.Count; i++)
            {
                var index = i;
                var playableChar = playableCharacterData[index];
                var item = Instantiate(itemPrefab, contentList);
                var playerRecord = SaveData.current.TryGetCharacterPlayRecord(index, out var record) ? record : null;
                var playedCount = playerRecord?.numOfGamePlayed ?? 0;
                item.characterNameText.text = $"{playableChar.name} [{playedCount}]";
                item.characterIcon.sprite = playableChar.uiIcon;
                item.iconAnimator.runtimeAnimatorController = playableChar.characterUIAnimatorController;


                item.characterSelectButton.onClick.AddListener(() => { SelectCharacterItem(index); });
                generatedItems.Add(item);
                item.Select(false);
            }

            SelectCharacterItem(currentSelectionIdx);
        }

        private void SelectCharacterItem(int itemIdx)
        {
            if (currentSelectionIdx != itemIdx)
            {
                generatedItems[currentSelectionIdx].Select(false);
            }

            currentSelectionIdx = itemIdx;
            generatedItems[currentSelectionIdx].Select(true);
        }

        public void Play()
        {
            SaveData.current.AddCharacterPlayRecordAndSave(currentSelectionIdx);
            SceneManager.LoadScene("Game");
        }
    }
}