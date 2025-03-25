using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arkademy.Data;
using Arkademy.Gameplay;
using Arkademy.Gameplay.Pickup;
using UnityEngine;
using UnityEngine.SceneManagement;
using Attribute = Arkademy.Data.Attribute;
using Random = UnityEngine.Random;

namespace Arkademy.Rift
{
    public class RiftController : MonoBehaviour
    {
        public static RiftController Instance;
        public static int? RiftSetup;
        public int difficulty;
        public int progress;
        public float passedTime;
        public Player playerPrefab;
        public bool riftStarted;

        public List<Gameplay.Character> spawnedEnemies = new();
        public List<Gameplay.Character> deadEnemies = new();
        public int spawnLimit;
        public float spawnInterval;
        public bool completed;
        [SerializeField] private float lastSpawn;
        private List<Race> _raceList = new();

        [SerializeField] private int eliteCounter;
        [SerializeField] private bool passed;

        [SerializeField] private int xpGain;
        [SerializeField] private int goldGain;
        [SerializeField] private float progressMultiplier;

        [SerializeField] private Healer[] healersPickupPrefabs;
        [SerializeField] private float healerDropChance;

        [SerializeField] private RiftMap map;
        [SerializeField] private RiftMapRenderer renderer;

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
            _raceList = Resources.LoadAll<Data.Scriptable.RaceObject>("")
                .Select(x => x.race).Where(x => !x.playable && x.spawnable).ToList();
            difficulty = RiftSetup ?? difficulty;
            progress = 0;
            eliteCounter = 5;
            map.Generate();
            renderer.Render();
            SpawnPlayer();
            SpawnEnemies();
            riftStarted = true;
        }

        private void SpawnEnemies()
        {
            var perRoomCount = 1000 / map.Rooms.Count;
            foreach (var room in map.Rooms.Values)
            {
                for (var i = 0; i < perRoomCount / 10; i++)
                {
                    SpawnEnemy(room.WorldPos(room.GetRandomOpenArea()));
                }
            }
        }

        private void SpawnPlayer()
        {
            Instantiate(playerPrefab).characterStartPosition = map.startPos;
        }

        private void SpawnEnemy(Vector3 position)
        {
            eliteCounter--;
            var isElite = eliteCounter <= 0;
            if (isElite) eliteCounter = 5;
            var spawnTarget = _raceList[Random.Range(0, _raceList.Count)];
            var enemy = Gameplay.Character.Create(spawnTarget, spawnTarget.CreateCharacterData(), 1);
            enemy.SetPosition(position);
            var difficultyLifeModifier = new Attribute.Modifier
            {
                attrType = Attribute.Type.Life,
                category = Attribute.Modifier.Category.Multiplication,
                value = Mathf.CeilToInt(Mathf.Pow(2, difficulty / 5f) * 10000)
            };
            var difficultyAttackModifier = new Attribute.Modifier
            {
                attrType = Attribute.Type.Attack,
                category = Attribute.Modifier.Category.Multiplication,
                value = Mathf.CeilToInt(Mathf.Pow(2, difficulty / 10f) * 10000)
            };
           
            enemy.Attributes.AddMod(difficultyLifeModifier);
            enemy.Attributes.AddMod(difficultyAttackModifier);
            if (isElite)
            {
                enemy.Attributes.AddMod(new Attribute.Modifier
                {
                    attrType = Attribute.Type.Life,
                    category = Attribute.Modifier.Category.Multiplication,
                    value = 30000,
                });

                enemy.Attributes.AddMod(new Attribute.Modifier
                {
                    attrType = Attribute.Type.Attack,
                    category = Attribute.Modifier.Category.Multiplication,
                    value = 20000
                });
                enemy.Attributes.AddMod(new Attribute.Modifier
                {
                    attrType = Attribute.Type.AttackSpeed,
                    category = Attribute.Modifier.Category.Multiplication,
                    value = 15000
                });
                enemy.Attributes.AddMod(new Attribute.Modifier
                {
                    attrType = Attribute.Type.MovSpeed,
                    category = Attribute.Modifier.Category.Multiplication,
                    value = 15000
                });
                enemy.graphic.spriteRenderer.color = Color.red;
            }

            
            enemy.OnDeath.AddListener(d =>
            {
                progress += Mathf.RoundToInt(10 * progressMultiplier * (isElite ? 2 : 1));
                xpGain += 10 * (1 + difficulty) * (isElite ? 2 : 1);
                goldGain += Mathf.CeilToInt(Random.Range(0.5f, 1.5f) * 10 * (1 + difficulty) * (isElite ? 2 : 1));
                spawnedEnemies.Remove(enemy);
                deadEnemies.Add(enemy);
                var dropHealer = Random.Range(0f, 1f) < healerDropChance || isElite;
                if (dropHealer)
                {
                    DropHealers(enemy.transform.position, Random.Range(1, isElite ? 5 : 1));
                }
            });
            spawnedEnemies.Add(enemy);
            var ai = enemy.GetOrAddComponent<CharacterAI>();
            ai.autoMove = true;
            ai.autoUseAbility = true;
            lastSpawn = Time.timeSinceLevelLoad;
        }

        private void DropHealers(Vector3 pos, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var healerPrefab = healersPickupPrefabs[Random.Range(0, healersPickupPrefabs.Length)];
                var healer = Instantiate(healerPrefab, pos + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                healer.healAmount += Mathf.CeilToInt(healer.healAmount * (difficulty * 0.1f));
            }
        }

        private void ClearAllEnemies()
        {
            foreach (var enemy in spawnedEnemies)
            {
                Destroy(enemy.gameObject);
            }

            foreach (var enemy in deadEnemies)
            {
                Destroy(enemy.gameObject);
            }
        }

        private async Task CompleteRift()
        {
            if (passedTime <= 300)
                passed = true;
            Session.currCharacterRecord.LastPlayed = DateTime.UtcNow;
            Session.currCharacterRecord.character.xp += xpGain;
            Session.currCharacterRecord.character.gold += goldGain;
            if (passed)
            {
                Session.currCharacterRecord.character.clearedRift = difficulty + Mathf.CeilToInt(300 - passedTime) / 20;
            }

            await Session.Save();
            SceneManager.LoadScene("Campus");
        }

        private async void Update()
        {
            if (!riftStarted || completed) return;
            passedTime += Time.deltaTime;
            if (passedTime > 300)
            {
                passed = false;
            }

            if (progress >= 1000)
            {
                completed = true;
                ClearAllEnemies();
                await CompleteRift();
                return;
            }


            // if (Time.timeSinceLevelLoad - lastSpawn >= spawnInterval && spawnedEnemies.Count <= spawnLimit)
            // {
            //     SpawnEnemy();
            // }
        }
    }
}