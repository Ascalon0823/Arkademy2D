using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Gameplay.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Data
{
    [Serializable]
    public class AbilityAvailability
    {
        public string abilityName;
        public int beginningLevel;
        public int maxLevel;
        public int xpPerLevel;
    }

    [Serializable]
    public class Race
    {
        private static Dictionary<string, Race> _raceCache = new Dictionary<string, Race>();
        public string displayName;
        public List<AttributeProfile> attributes = new();
        public bool playable;
        public bool spawnable;
        public Gameplay.Character behaviourPrefab;
        public RuntimeAnimatorController animationController;
        public bool facingLeft;

        public List<EquipmentSlot> slots = new();
        public List<AbilityAvailability> abilities = new();
        public static Race GetRace(string name)
        {
            if (!_raceCache.TryGetValue(name, out var cached))
            {
                cached = Resources.Load<Scriptable.RaceObject>(name).race;
                _raceCache[name] = cached;
            }

            return cached;
        }

        public Character CreateCharacterData(string charaName = "")
        {
            return new Character
            {
                displayName = string.IsNullOrEmpty(charaName) ? displayName : charaName,
                raceName = displayName,
                equipmentSlots = slots
            };
        }
    }
}