using System;
using System.Collections.Generic;
using System.Linq;
using Midterm.Character;
using Midterm.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Midterm.Field
{
    [Serializable]
    public class WaveData
    {
        public float spawnInterval;
        public int spawnLimit;
        public List<Enemy> spawnableEnemies;
    }

    [Serializable]
    public class SpecialSpawnPattern
    {
        public enum SpecialSpawnType
        {
            Surround,
            Swarm
        }

        public int minimumWaveCount;
        public float delay;
        public int waveInterval;
        public Enemy enemyPrefab;

        public SpecialSpawnType specialSpawnType;
        public bool spawned;
        public int lastSpawnedWave;

        public bool CanSpawn(int waveCount, float waveTime)
        {
            if (lastSpawnedWave != waveCount)
            {
                spawned = false;
            }

            return !spawned && (waveCount == minimumWaveCount || (waveCount - minimumWaveCount) % waveInterval == 0)
                            && waveTime >= delay;
        }

        public void Spawn(int waveCount)
        {
            switch (specialSpawnType)
            {
                case SpecialSpawnType.Surround:
                    for (var i = 0; i < 30; i++)
                    {
                        var pos = Player.Player.Local.currCharacter.transform.position
                                  + Quaternion.Euler(0, 0, i / 30f * 360f) * Vector3.up * 10;
                        var spawnedEnemey = GameObject.Instantiate(enemyPrefab, pos, Quaternion.identity);
                        spawnedEnemey.autoDespawn = true;
                        spawnedEnemey.remainingTime = 10f;
                        spawnedEnemey.character.moveSpeed = 1f;
                        spawnedEnemey.character.maxLife = Mathf.FloorToInt(500 * (1+waveCount/2f));
                        spawnedEnemey.character.life = Mathf.FloorToInt(500 * (1+waveCount/2f));
                        spawnedEnemey.fixedMove = true;
                        spawnedEnemey.fixMovingDir =
                            (Player.Player.Local.currCharacter.transform.position - pos).normalized;
                        spawnedEnemey.energyDropRate = 0.0f;
                    }

                    break;
                case SpecialSpawnType.Swarm:
                    var from = Player.Player.Local.currCharacter.transform.position +
                               (Vector3)Random.insideUnitCircle.normalized * 10f;
                    var moveDir = (Player.Player.Local.currCharacter.transform.position - from).normalized;
                    for (var i = 0; i < 30; i++)
                    {
                        var spawnedEnemey = GameObject.Instantiate(enemyPrefab,
                            from + (Vector3)Random.insideUnitCircle * 3f, Quaternion.identity);
                        spawnedEnemey.autoDespawn = true;
                        spawnedEnemey.remainingTime = 3f;
                        spawnedEnemey.character.moveSpeed = 6f;
                        spawnedEnemey.character.maxLife = 1;
                        spawnedEnemey.character.life = 1;
                        spawnedEnemey.fixedMove = true;
                        spawnedEnemey.fixMovingDir = moveDir;
                        spawnedEnemey.energyDropRate = 0.0f;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            spawned = true;
            lastSpawnedWave = waveCount;
        }
    }

    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;
        public float actualTime;
        public float totalTime;
        public float waveTime;
        public int waveCount;
        [Range(0f, 10f)] public float speed;

        public Player.Player player;
        public float lastSpawnTime;
        public List<Character.Character> spawnedEnemies = new();
        public float despawnDistance;
        public List<Ability> availableAbilities = new List<Ability>();

        public List<WaveData> waveData = new List<WaveData>();
        public List<SpecialSpawnPattern> specialSpawnPatterns = new List<SpecialSpawnPattern>();
        public LevelUpUI levelUpUI;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            player.currCharacter.onDead.AddListener(() =>
            {
                player.record.gold += waveCount;
                SaveEngine.Instance.Save(player.record);
                SceneManager.LoadScene("Midterm/Player/Title");
            });
            player.currCharacter.GetComponent<Level>().onLevelUp.AddListener(level =>
            {
                levelUpUI.levelUpEvent.Enqueue(level);
                levelUpUI.Toggle(true);
            });
            player.record.playCount++;
        }

        private void Update()
        {
            actualTime = Time.timeSinceLevelLoad;
            totalTime += Time.deltaTime * speed;
            waveTime = totalTime % 60;
            waveCount = 1 + Mathf.FloorToInt(totalTime / 60);
            DespawnFarEnemy();
            SpawnEnemy(waveData[Mathf.Clamp(waveCount - 1, 0, waveData.Count - 1)]);
        }

        public float GetCurrWaveProgress()
        {
            return waveTime / 120f;
        }

        private void DespawnFarEnemy()
        {
            var toDespawn = new List<Character.Character>();
            foreach (var enemy in spawnedEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, player.currCharacter.transform.position) >
                    player.currCamera.camRect.size.magnitude / 2f * despawnDistance)
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

        private void SpawnEnemy(WaveData data)
        {
            foreach (var spawnPattern in specialSpawnPatterns)
            {
                if (spawnPattern.CanSpawn(waveCount, waveTime))
                {
                    spawnPattern.Spawn(waveCount);
                }
            }

            if (Time.timeSinceLevelLoad - lastSpawnTime <= data.spawnInterval) return;
            if (spawnedEnemies.Count >= data.spawnLimit) return;
            lastSpawnTime = Time.timeSinceLevelLoad;
            var pos = player.currCamera.GetRandomPositionOutSideScreen(despawnDistance);
            var enemyPrefab = data.spawnableEnemies[Random.Range(0, data.spawnableEnemies.Count)];
            var spawned = Instantiate(enemyPrefab, pos, Quaternion.identity);
            spawnedEnemies.Add(spawned.character);
            spawned.character.maxLife = Mathf.FloorToInt(spawned.character.maxLife * (1 + (waveCount-1) / 2f));
            spawned.character.life = spawned.character.maxLife;
            spawned.character.power = Mathf.FloorToInt(spawned.character.power * (1 + (waveCount-1) / 4f));
            if (Random.Range(0f, 1f) < 0.05f)
            {
                spawned.character.moveSpeed *= 1.5f;
            }
        }
    }
}