using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.Behaviour
{
    public class Game : MonoBehaviour
    {
        [Serializable]
        public class Session
        {
            [Serializable]
            public class PlayerSetup
            {
                public PlayerRecord playerRecord;
                public CharacterRecord characterRecord;
            }

            public List<PlayerSetup> playerSetups;
        }

        public static Session UseSession;
        private static Game _current;
        [SerializeField] private Player playerPrefab;
        public List<Player> players = new List<Player>();


        private void Awake()
        {
            if (_current && _current != this)
            {
                Destroy(gameObject);
                return;
            }

            _current = this;
        }

        private void Start()
        {
            if (UseSession == null)
            {
                UseSession = GetLastSession();
            }

            foreach (var setup in UseSession.playerSetups)
            {
                var player = Instantiate(playerPrefab);
                players.Add(player);
                player.Setup(setup);
            }
        }

        private static Session GetLastSession()
        {
            var playerRecord = PlayerRecord.LoadAll().First();

            var characterRecord = playerRecord.characterRecords.OrderByDescending(x => x.LastPlayed).First();
            return new Session
            {
                playerSetups = new()
                    { new Session.PlayerSetup { playerRecord = playerRecord, characterRecord = characterRecord } }
            };
        }


        public static void StartGame(Session session)
        {
            UseSession = session;
            foreach (var playerSetup in session.playerSetups)
            {
                playerSetup.playerRecord.LastPlayed = DateTime.UtcNow;
                playerSetup.characterRecord.LastPlayed = DateTime.UtcNow;
                playerSetup.playerRecord.Save();
            }

            SceneManager.LoadScene("Arkademy/Scenes/Game");
        }
    }
}