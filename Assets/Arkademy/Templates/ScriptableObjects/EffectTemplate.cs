using System;
using UnityEngine;

namespace Arkademy.Templates
{
    public abstract class EffectTemplate<T> : ScriptableObject where T : Data.Effect
    {
        public T effect;
        public Sprite icon;

        private void OnEnable()
        {
            if (effect != null)
                effect.templateName = name;
        }
    }
}