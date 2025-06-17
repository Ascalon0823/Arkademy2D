using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Field
{
    public class WaveManager : MonoBehaviour
    {
        public float actualTime;
        public float totalTime;
        public float waveTime;
        public int waveCount;
        [Range(0f,10f)]
        public float speed;

        public Player.Player player;

        public Character.Character enemyPrefab;
        public float spawnInterval;
        public int spawnLimit;
        public float lastSpawnTime;
        public List<Character.Character> spawnedEnemies = new();
        public float despawnDistance;
        private void Update()
        {
            actualTime = Time.timeSinceLevelLoad;
            totalTime += Time.deltaTime * speed;
            waveTime = totalTime % 120;
            waveCount = 1+Mathf.FloorToInt(totalTime / 120);
            DespawnFarEnemy();
            SpawnEnemy();
        }

        private void DespawnFarEnemy()
        {
            var toDespawn = new List<Character.Character>();
            foreach (var enemy in spawnedEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, player.currCharacter.transform.position) >
                    player.currCamera.camRect.size.magnitude/2f *despawnDistance)
                {
                    toDespawn.Add(enemy);
                }
                
            }

            foreach (var despawn in toDespawn)
            {
                spawnedEnemies.Remove(despawn);
                Destroy(despawn.gameObject);
            }

            foreach (var dead in spawnedEnemies.Where(x => x.life <= 0).ToList())
            {
                spawnedEnemies.Remove(dead);
            }
        }

        private void SpawnEnemy()
        {
            if (Time.timeSinceLevelLoad - lastSpawnTime <= spawnInterval) return;
            if (spawnedEnemies.Count >= spawnLimit) return;
            lastSpawnTime = Time.timeSinceLevelLoad;
            var pos = player.currCamera.GetRandomPositionOutSideScreen(despawnDistance);
            var spawned = Instantiate(enemyPrefab,pos,Quaternion.identity);
            spawnedEnemies.Add(spawned);
        }
    }
}