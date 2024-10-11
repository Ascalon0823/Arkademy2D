using System;
using Arkademy.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy
{
    [CreateAssetMenu(fileName = "New Character Base", menuName = "Data/Add Character Base", order = 0)]
    public class CharacterBase : ScriptableObject
    {
    }

    [Serializable]
    public struct CharacterData
    {
        [Serializable]
        public struct Attr
        {
            public enum Category
            {
                XpGain,
                Life,
                Stamina,
                Source,
                MoveSpeed,
                Luck,
                Strength,
                Constitution,
                Dexterity,
                Wisdom,
                Faith,
                Charisma,
            }

            public Category category;
            public int value;
        }

        [Serializable]
        public struct Mastery
        {
            public EquipmentData.Type equipType;
            public int value;
        }

        public Attr[] attributes;
        public Mastery[] masteries;
    }
}