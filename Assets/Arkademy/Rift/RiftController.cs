using System;
using Arkademy.Gameplay;
using UnityEngine;
namespace Arkademy.Rift
{
    public class RiftController :MonoBehaviour
    {
        public static RiftController Instance;
        public static int RiftSetup;
        public int difficulty;
        public int progress;
        public Player playerPrefab;
        
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            difficulty = RiftSetup;
            progress = 0;
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            Instantiate(playerPrefab);
        }
    }
}
