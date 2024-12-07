using System;
using System.Linq;
using Arkademy.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arkademy.CharacterCreation
{
    public class CharacterCreationMenu : MonoBehaviour
    {
        public Character currentCharacter;
        [SerializeField] private TMP_InputField nameInputField;

        [SerializeField] private Button confirm;
        [SerializeField] private Button cancel;

        [SerializeField] private Race[] races;
        private void Awake()
        {
            nameInputField.onValueChanged.RemoveAllListeners();
            nameInputField.onValueChanged.AddListener(s =>
            {
                confirm.interactable = !string.IsNullOrEmpty(s);
                currentCharacter.displayName = s;
            });
            confirm.onClick.RemoveAllListeners();
            confirm.interactable = false;
            confirm.onClick.AddListener(OnConfirm);
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(OnCancel);
        }

        public void OnEnable()
        { 
            races = Resources.LoadAll<Race>("");
            races = races.Where(x => x.playable).ToArray();
            SelectRace(0);
        }

        public void SelectRace(int idx)
        {
            currentCharacter = races[idx].CreateCharacter();
            currentCharacter.displayName = nameInputField.text;
        }

        public void OnConfirm()
        {
            var characterRecord = new CharacterRecord
            {
                character = currentCharacter,
                time = new GameTime()
            };
            Session.currPlayerRecord.characterRecords.Add(characterRecord);
            Session.currPlayerRecord.Save();
            Session.currCharacterRecord = characterRecord;
            characterRecord.LastPlayed = DateTime.UtcNow;
            SceneManager.LoadScene("Campus");
        }

        public void OnCancel()
        {
            SceneManager.LoadScene("Arkademy/Title/Title");
        }
    }
}