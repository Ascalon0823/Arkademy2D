using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.Effect
{
    [Serializable]
    public abstract class Effect
    {
        public CharacterBehaviour target;
        public GameObject giver;
        public float duration;
        public bool updated;
        public bool updateImmediate;

        public abstract Effect Dup();

        public virtual void Update()
        {
            updated = true;
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Ended();
            }
        }

        public virtual void Ended()
        {
            target?.Effects.Remove(this);
        }

        public virtual bool CanApplyTo(CharacterBehaviour t)
        {
            return true;
        }
    }
}