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
            [Serializable]
            public struct ApplicationEvent
            {
                public Gameplay.Character dealer;
                public Gameplay.Ability.AbilityBase ability;
                public Gameplay.Character receiver;
            }
            public abstract void Apply(ApplicationEvent e);
        }

        [Serializable]
        public class DamageEffect : Effect
        {
            public int damage;
            public int maxDamage;
            public int numOfHit;
            public int maxNumOfHit;

            public override void Apply(ApplicationEvent e)
            {
                
            }
        }
    }

  
}