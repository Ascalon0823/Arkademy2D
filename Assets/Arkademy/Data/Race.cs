using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Gameplay.Ability;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Race
    {
        private static Dictionary<string, Race> _raceCache = new Dictionary<string, Race>();
        public string displayName;
        public List<Attribute> attributes = new();
        public bool playable;
        public bool spawnable;
        public Gameplay.Character behaviourPrefab;
        public RuntimeAnimatorController animationController;
        public bool facingLeft;

        public List<AbilityBase> abilities=new List<AbilityBase>();
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
            };
        } 
    }
}