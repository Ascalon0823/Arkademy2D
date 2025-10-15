using System.Collections.Generic;
using Midterm.Character;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Midterm.Player
{
    public class PlayableCharacterSelection : MonoBehaviour
    {
        public PlayableCharacterSelectionItem currSelection;
        public Title title;
        public RectTransform characterHolder;

        [SerializeField]
        private List<PlayableCharacterSelectionItem> spawned = new List<PlayableCharacterSelectionItem>();

        [SerializeField] private PlayableCharacterSelectionItem itemPrefab;

        [SerializeField] private Button unlockButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI goldText;

        public void Toggle(bool active)
        {
            if (active)
            {
                PopulateList();
            }

            gameObject.SetActive(active);
        }

        public void Select(PlayableCharacterSelectionItem selected)
        {
            currSelection = selected;
            var unlocked = title.saveData.unlockedCharacters.Contains(selected.data.internalName);
            unlockButton.interactable = !unlocked && title.saveData.gold >= selected.data.unlockPrice;
            confirmButton.interactable = unlocked;
        }

        public void Unlock()
        {
            if (title.saveData.gold < currSelection.data.unlockPrice) return;
            title.saveData.gold -= currSelection.data.unlockPrice;
            goldText.text = $"${title.saveData.gold}";
            title.saveData.unlockedCharacters.Add(currSelection.data.internalName);
            SaveEngine.Instance.Save(title.saveData);
            currSelection.SetupAs(this,currSelection.data, true);
            Select(currSelection);
        }

        public void PopulateList()
        {
            if (title.saveData.unlockedCharacters == null || title.saveData.unlockedCharacters.Count == 0)
            {
                title.saveData.unlockedCharacters = new List<string>() { "test" };
            }

            foreach (var item in spawned)
            {
                Destroy(item.gameObject);
            }

            spawned.Clear();
            currSelection = null;
            foreach (var character in PlayableCharacterData.GetAll())
            {
                var item = Instantiate(itemPrefab, characterHolder);
                item.SetupAs(this, character, title.saveData.unlockedCharacters.Contains(character.internalName));
                if (!currSelection)
                {
                    Select(item);
                }

                spawned.Add(item);
            }
            goldText.text = $"${title.saveData.gold}";
        }

        public void Confirm()
        {
            Player.SelectedCharacterInternalName = currSelection.data.internalName;
            SceneManager.LoadScene("Midterm/Player/Game");
        }
    }
}