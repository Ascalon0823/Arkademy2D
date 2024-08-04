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
            public CharacterBehaviour characterPrefab;
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

        public List<PlayableCharacterData> playableCharacterData = new List<PlayableCharacterData>();
        public List<AbilityData> abilityData = new List<AbilityData>();

        public static Database GetDatabase()
        {
            return Resources.LoadAll<Database>("Database").FirstOrDefault();
        }

    }
}