using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class MainMenu : MonoBehaviour
    {
        private static MainMenu _instance;
        [SerializeField] private List<PlayerRecord> selectedPlayerRecords = new();
        [SerializeField] private List<PlayerRecord> playerRecords = new();
        [SerializeField] private LandingPage landingPage;
        [SerializeField] private PlayerRecordCreation playerRecordCreation;
        [SerializeField] private CharacterList characterList;


        public void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            playerRecords = PlayerRecord.LoadAll();
            Debug.Log($"{playerRecords.Count} player record found");
            landingPage.gameObject.SetActive(true);
            playerRecordCreation.gameObject.SetActive(false);
            characterList.gameObject.SetActive(false);
        }

        public static void AddPlayerRecord(PlayerRecord record)
        {
            _instance.playerRecords.Add(record);
            _instance.SaveAllRecords();
        }

        public static bool TryGetLatestPlayerRecord(out PlayerRecord latest)
        {
            latest = null;
            if (!_instance.playerRecords.Any()) return false;
            latest = _instance.playerRecords.First();
            return true;
        }


        public static void AddSelectedPlayerRecord(PlayerRecord playerRecord)
        {
            _instance.selectedPlayerRecords.Add(playerRecord);
        }

        public void LandingPagePressed()
        {
            if (!TryGetLatestPlayerRecord(out var record))
            {
                playerRecordCreation.gameObject.SetActive(true);
                return;
            }
            AddSelectedPlayerRecord(record);
            playerRecordCreation.gameObject.SetActive(false);
            characterList.gameObject.SetActive(true);
            characterList.SetCharacters(record.characterRecords);
        }

        private void SaveAllRecords()
        {
            foreach (var playerRecord in playerRecords)
            {
                playerRecord.Save();
            }
        }
    }
}