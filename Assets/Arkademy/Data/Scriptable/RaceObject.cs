using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.Data.Scriptable
{
    [CreateAssetMenu(fileName = "Race", menuName = "Data/Race", order = 0)]
    public class RaceObject : ScriptableObject
    {
        [Serializable]
        private class AttrDisplay
        {
            public Attribute attribute;
            public int storeValue;
            public float realValue;
        }

        public Race race;
        public bool playable;
        public List<Attribute.Modifier> raceModifiers = new();
        [Header("Behaviour")] public Gameplay.Character behaviourPrefab;
        public RuntimeAnimatorController animationController;
        public bool facingRight;
        
        
        
        [SerializeField] private List<AttrDisplay> displays = new();

        private void OnValidate()
        {
            displays.Clear();
            var mods = new Dictionary<Attribute.Type, List<Attribute.Modifier>>();
            foreach (var mod in raceModifiers)
            {
                if (!mods.TryGetValue(mod.attrType, out var modList))
                {
                    modList = new();
                    mods[mod.attrType] = modList;
                }

                mods[mod.attrType].Add(mod);
            }

            foreach (var attr in race.attributes)
            {
                var ad = new AttrDisplay
                {
                    attribute = attr.Copy()
                };
                if (mods.TryGetValue(attr.type, out var modList))
                {
                    foreach (var mod in modList)
                    {
                        ad.attribute.AddMod(mod);
                    }
                }

                ad.realValue = ad.attribute.GetValue();
                ad.storeValue = ad.attribute.GetBaseValue();
                displays.Add(ad);
            }
        }
    }
}