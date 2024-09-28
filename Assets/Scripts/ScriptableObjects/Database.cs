using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy
{
    [CreateAssetMenu(fileName = "Database", menuName = "Database/CreateNewDatabase", order = 1)]
    public class Database : ScriptableObject
    {
        [Serializable]
        public class CharacterData
        {
            public string name;
            public Sprite uiIcon;
            public RuntimeAnimatorController characterUIAnimatorController;
            public RuntimeAnimatorController animatorController;
            public int[] beginningAbilityIdx;
            public int life;
        }
        [Serializable]
        public class PlayableCharacterData : CharacterData
        {
            public bool unlocked;
        }

        [Serializable]
        public class AbilityData
        {
            public string name;
            public Ability prefab;
            public Sprite uiIcon;
        }

        [Serializable]
        public class SpellData
        {
            public string name;
            public string spellKey;
            public Spell prefab;
            public Sprite uiIcon;
        }

        [Serializable]
        public class EnemyData
        {
            public string name;
            public RuntimeAnimatorController controller;
            public Sprite uiIcon;
            public int health;
            public int power;
            public int speed;
            public int xp;
        }

        [Serializable]
        public class StageData
        {
            public string stageName;
            public WaveData[] waveData;
        }

        [Serializable]
        public class WaveData
        {
            public int minimumEnemy;
            public float spawnInterval;
            public int[] spawnableEnemy;
        }

        [Serializable]
        public class BossData
        {
            public float timing;
        }
        [Serializable]
        public class EventData
        {
            
        }

        public List<PlayableCharacterData> playableCharacterData = new List<PlayableCharacterData>();
        public List<AbilityData> abilityData = new List<AbilityData>();
        public List<SpellData> spellData = new List<SpellData>();
        public List<EnemyData> enemyData = new List<EnemyData>();
        public List<StageData> stageData = new List<StageData>();

        public static Database GetDatabase()
        {
            return Resources.LoadAll<Database>("Database").FirstOrDefault();
        }

    }
}