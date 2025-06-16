using System;
using Arkademy.Gameplay;
using UnityEngine;

namespace Arkademy.Campus
{
    public class CampusController : MonoBehaviour
    {
        public static CampusController Instance;
        public Player playerPrefab;
        public GameObject campusLevelPrefab;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            Instantiate(campusLevelPrefab);
            Instantiate(playerPrefab);
        }
    }
}