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
        public int startingWave;
        public float secondsPlayed;

        //public List<EnemyBehaviour> spawnedEnemies = new();
        public Dictionary<int, List<EnemyBehaviour>> spawnedEnemies = new();
        public List<CharacterBehaviour> spawnedCharacters = new List<CharacterBehaviour>();
        public float enemySpawnInterval;
        public float lastEnemySpawn;
        public int currWave;
        private int _prevWave;
        public float speedMultiplier;

        public float enemyDamageNegation = 0;

        public EnemyBehaviour enemyPrefab;
        public bool bossSpawned;

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
            currWave = startingWave + wave;
            if (currWave != _prevWave)
                bossSpawned = false;
            _prevWave = currWave;
            currWaveData = currentStageData.waveData[currWave - 1];
            enemySpawnInterval = currWaveData.spawnInterval;
            if (!spawnedEnemies.TryGetValue(currWave, out var list) || list.Count < currWaveData.minimumEnemy)
            {
                SpawnEnemy();
            }
            
            var bossEvents = currWaveData.bossEvents;
            if (bossEvents != null && !bossSpawned)
            {
                foreach (var bossEvent in bossEvents)
                {
                    SpawnBoss(bossEvent);
                }

                bossSpawned = true;
            }
        }

        public void SpawnBoss(Database.BossEvent bossEvent)
        {
            if (!spawnedEnemies.TryGetValue(currWave, out var list))
            {
                list = new List<EnemyBehaviour>();
            }

            var bossIdx = bossEvent.bossIdx;
            var bossData = Database.GetDatabase().bossData[bossIdx];
            var enemy = Instantiate(enemyPrefab);
            enemy.animator.runtimeAnimatorController = bossData.controller;
            enemy.life = bossData.health;
            enemy.gameObject.name = bossData.name;
            enemy.xpDrop = bossData.xp;
            enemy.moveSpeed = bossData.speed / 100f;
            enemy.contactDamage.damage = bossData.power;
            list.Add(enemy);
            enemy.transform.position = Player.Cam.GetRandomPositionOutSideScreen(1f);
            enemy.onDeath.AddListener(e => { list.Remove(e as EnemyBehaviour); });
            enemy.transform.localScale = Vector3.one * bossData.scale;
            enemy.life *= bossData.scaleHpWithPlayer ? Mathf.Max(1, Player.Chara.level) : 1;
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
            var enemy = Instantiate(enemyPrefab);
            enemy.animator.runtimeAnimatorController = enemyData.controller;
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