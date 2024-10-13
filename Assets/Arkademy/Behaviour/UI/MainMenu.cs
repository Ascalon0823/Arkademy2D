using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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

        public static void PlayerStarted()
        {
            if (!TryGetLatestPlayerRecord(out var record))
            {
                _instance.playerRecordCreation.Activate(_instance.CheckLandingPageShouldBeActive,
                    newRecord =>
                    {
                        AddPlayerRecord(newRecord);
                        AddSelectedPlayerRecord(newRecord);
                        _instance.ActivateCharacterList(newRecord);
                    });
                return;
            }
            AddSelectedPlayerRecord(record);
            _instance.ActivateCharacterList(record);
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


        private void ActivateCharacterList(PlayerRecord record)
        {
            characterList.Activate(record.characterRecords, CheckLandingPageShouldBeActive,
                characterRecord =>
                {
                    var session = new Game.Session();
                    session.playerSetups = new List<Game.Session.PlayerSetup>();
                    session.playerSetups.Add(new Game.Session.PlayerSetup
                    {
                        characterRecord = characterRecord,
                        playerRecord = record
                    });
                    Game.StartGame(session);
                });
        }

        private void CheckLandingPageShouldBeActive()
        {
            if (!characterList.gameObject.activeInHierarchy && !playerRecordCreation.gameObject.activeInHierarchy)
            {
                landingPage.gameObject.SetActive(true);
            }
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