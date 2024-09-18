using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy
{
    public class StageBehaviour : MonoBehaviour
    {
        public static StageBehaviour Current;
        public Database.StageData currentStageData;
        public Database.WaveData currWaveData;

        public float secondsPlayed;

        //public List<EnemyBehaviour> spawnedEnemies = new();
        public Dictionary<int, List<EnemyBehaviour>> spawnedEnemies = new();
        public List<CharacterBehaviour> spawnedCharacters = new List<CharacterBehaviour>();
        public float enemySpawnInterval;
        public float lastEnemySpawn;
        public int currWave;
        public float speedMultiplier;

        public float enemyDamageNegation = 0;
        private void Awake()
        {
            if (Current && Current != this)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            currentStageData = Database.GetDatabase().stageData[Player.UsingStageDBIdx ?? 0];
        }

        private void Update()
        {
            secondsPlayed += Time.deltaTime * speedMultiplier;
            var wave = Mathf.FloorToInt(1 + secondsPlayed / 60);
            currWave = wave;
            currWaveData = currentStageData.waveData[currWave - 1];

            if (!spawnedEnemies.TryGetValue(currWave, out var list) || list.Count < currWaveData.minimumEnemy)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            if (secondsPlayed - lastEnemySpawn < enemySpawnInterval) return;

            if (!spawnedEnemies.TryGetValue(currWave, out var list))
            {
                list = new List<EnemyBehaviour>();
            }

            var enemyPrefabIdx = currWaveData.spawnableEnemy[Random.Range(0, currWaveData.spawnableEnemy.Length)];
            var enemyDb = Database.GetDatabase().enemyData;
            var enemyData = enemyDb[enemyPrefabIdx];
            var enemy = Instantiate(enemyData.prefab);
            enemy.life = enemyData.health;
            enemy.gameObject.name = enemyData.name;
            enemy.xpDrop = enemyData.xp;
            enemy.moveSpeed = enemyData.speed / 100f;
            enemy.contactDamage.damage = enemyData.power;
            list.Add(enemy);
            enemy.transform.position = Player.Cam.GetRandomPositionOutSideScreen(1f);
            enemy.onDeath.AddListener(e => { list.Remove(e as EnemyBehaviour); });
            lastEnemySpawn = secondsPlayed;
        }
    }
}