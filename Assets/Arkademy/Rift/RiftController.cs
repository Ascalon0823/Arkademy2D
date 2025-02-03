using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.CharacterCreation;
using Arkademy.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Rift
{
    public class RiftController :MonoBehaviour
    {
        public static RiftController Instance;
        public static int RiftSetup;
        public int difficulty;
        public int progress;
        public float passedTime;
        public Player playerPrefab;
        public bool riftStarted;

        public List<Character> spawnedEnemies = new();
        public List<Character> deadEnemies = new();
        public int spawnLimit;
        public float spawnInterval;
        public bool completed;
        [SerializeField] private float lastSpawn;
        private List<Race> _raceList = new();
        
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
            _raceList = Resources.LoadAll<Race>("").Where(x => !x.playable).ToList();
            difficulty = RiftSetup;
            progress = 0;
            SpawnPlayer();
            riftStarted = true;
        }

        private void SpawnPlayer()
        {
            Instantiate(playerPrefab);
        }

        private void SpawnEnemy()
        {
            var spawnTarget = _raceList[Random.Range(0, _raceList.Count)];
            var enemy = Character.Create(spawnTarget.CreateCharacter(), 1);
            enemy.SetPosition(Player.LocalPlayer.GetRandomPosArrandCharacter(1f));
            enemy.OnDeath += () =>
            {
                progress += 10;
                spawnedEnemies.Remove(enemy);
                deadEnemies.Add(enemy);
            };
            spawnedEnemies.Add(enemy);
            var ai = enemy.GetOrAddComponent<CharacterAI>();
            ai.autoMove = true;
            ai.autoUseAbility = true;
            lastSpawn = Time.timeSinceLevelLoad;
        }

        private void SetEnemyDifficulty()
        {
            foreach (var enemy in spawnedEnemies)
            {
                
            }
        }

        private void Update()
        {
            if (!riftStarted ||completed) return;
            passedTime += Time.deltaTime;
            if (passedTime > 300)
            {
                //Failed
                completed = true;
            }

            if (progress >= 1000)
            {
                completed = true;
            }


            if (Time.timeSinceLevelLoad - lastSpawn >= spawnInterval && spawnedEnemies.Count <= spawnLimit)
            {
                SpawnEnemy();
            }
        }
    }
}
