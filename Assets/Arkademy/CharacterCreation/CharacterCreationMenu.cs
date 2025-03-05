using System;
using System.Linq;
using System.Threading.Tasks;
using Arkademy.Backend;
using Arkademy.Data;
using Arkademy.Data.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CharacterRecord = Arkademy.Data.CharacterRecord;

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
            confirm.interactable = false;
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(OnCancel);
        }

        public void OnEnable()
        {
            races = Resources.LoadAll<RaceObject>("").Select(x => x.race)
                .Where(x => x.playable).ToArray();
            SelectRace(0);
        }

        public void SelectRace(int idx)
        {
            currentCharacter = races[idx].CreateCharacterData();
            currentCharacter.displayName = nameInputField.text;
        }

        public async void OnConfirm()
        {
            var characterRecord = new CharacterRecord
            {
                character = currentCharacter,
                CreationTime = DateTime.UtcNow,
                LastPlayed = DateTime.UtcNow
            };
            Session.currPlayerRecord.characterRecords.Add(characterRecord);
            Session.currCharacterRecord = characterRecord;
            await Session.Save();
            SceneManager.LoadScene("Campus");
        }

        public void OnCancel()
        {
            SceneManager.LoadScene("Arkademy/Title/Title");
        }
    }
}