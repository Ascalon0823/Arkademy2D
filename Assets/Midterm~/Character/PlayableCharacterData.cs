using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Midterm.Character
{
    [CreateAssetMenu(fileName = "PlayableCharacter Data", menuName = "New Playable Character Data", order = 0)]
    public class PlayableCharacterData : ScriptableObject
    {
        public string internalName;
        public string displayName;
        public bool unlocked;
        public int unlockPrice;
        public Sprite icon;
        public RuntimeAnimatorController animator;

        public int life;
        public float power;
        public float speed;
        public float atkSpeed;

        public List<Ability> initialAbilities;
        private static List<PlayableCharacterData> characterDatabase;

        public static PlayableCharacterData Get(string internalName)
        {
            return GetAll().FirstOrDefault(x => x.internalName == internalName) ?? characterDatabase[0];
        }

        public static List<PlayableCharacterData> GetAll()
        {
            if (characterDatabase == null)
            {
                characterDatabase = Resources.LoadAll<PlayableCharacterData>("").ToList();
            }

            return characterDatabase;
        }
    }
}