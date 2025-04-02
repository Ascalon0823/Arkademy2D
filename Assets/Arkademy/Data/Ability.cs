using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Ability
    {
        public enum TargetType
        {
            None,
            Direction,
            Position,
            Character
        }

        [Serializable]
        public struct Cost
        {
            public int energy;
        }

        public string name;
        public string displayName;
        public string description;
        public TargetType targetType;
        public Cost cost;
        public float useTime;
        public float cooldown;


        [Serializable]
        public abstract class Effect
        {
            [Flags]
            public enum CharacterType
            {
                
                Self = 1<<1,
                Friendly = 1<<2,
                Enemy = 1<<3
            }

            [Serializable]
            public struct EffectEventData
            {
                public Gameplay.Character dealer;
                public Gameplay.Ability.AbilityBase ability;
                public Gameplay.Character receiver;
            }

            public CharacterType affects;
            public abstract void Apply(EffectEventData e);
        }

        [Serializable]
        public class DamageEffect : Effect
        {
            public int damage;
            public int maxDamage;
            public int numOfHit;
            public int maxNumOfHit;

            public override void Apply(EffectEventData e)
            {
            }
        }
    }
}