using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Arkademy
{
    [CreateAssetMenu(fileName = "Database", menuName = "Database/CreateNewDatabase", order = 1)]
    public class Database : ScriptableObject
    {
        [Serializable]
        public class PlayableCharacterData
        {
            public string name;
            public Sprite uiIcon;
            public AnimatorController characterUIAnimatorController;
            public bool unlocked;
        }

        public List<PlayableCharacterData> playableCharacterData = new List<PlayableCharacterData>();

        public static Database GetDatabase()
        {
            return Resources.LoadAll<Database>("Database").FirstOrDefault();
        }

    }
}