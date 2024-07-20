using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class StageBehaviour : MonoBehaviour
    {
        public static StageBehaviour Current;
        public float secondsPlayed;
        public int enemyCap;
        public List<EnemyBehaviour> spawnedEnemies = new();
        public List<CharacterBehaviour> spawnedCharacters = new List<CharacterBehaviour>();
        public EnemyBehaviour enemyPrefab;
        public float enemySpawnInterval;
        public float lastEnemySpawn;


        private void Awake()
        {
            if (Current && Current != this)
            {
                Destroy(gameObject);
                return;
            }
            Current =this;
        }

        private void Update()
        {
            secondsPlayed += Time.deltaTime;
            if (spawnedEnemies.Count < enemyCap)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            if (secondsPlayed - lastEnemySpawn < enemySpawnInterval) return;
            var enemy = Instantiate(enemyPrefab);
            spawnedEnemies.Add(enemy);
            enemy.transform.position = PlayerBehaviour.PlayerCam.GetRandomPositionOutSideScreen(1f);
            enemy.onDeath.AddListener(e =>
            {
                spawnedEnemies.Remove(e as EnemyBehaviour);
            });
            lastEnemySpawn = secondsPlayed;
        }
    }
}