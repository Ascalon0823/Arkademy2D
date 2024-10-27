using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public abstract class Effect
    {
        [Header("Runtime")]
        public float duration;
        public Character target;
        public Character dealer;

        public virtual void Update(float delta)
        {
            if (duration <= 0) return;
            duration -= delta;
            UpdatePayload(delta);
            if (duration <= 0)
            {
                Removed();
            }
        }

        protected virtual void UpdatePayload(float delta)
        {
        }

        public virtual bool AppliedTo(Character character)
        {
            target = character;
            return true;
        }

        public abstract void Removed();
    }

    [Serializable]
    public abstract class Effect<T> : Effect where T : Effect<T>
    {
        public abstract T Copy();
    }

    [Serializable]
    public class ModifierEffect : Effect<ModifierEffect>
    {
        [Header("Edit")]
        public string targetAttribute;
        public string modifierSourceString;
        public List<Modifier> modifiers = new List<Modifier>();

        [Header("Runtime")] [SerializeField] private List<Modifier> created = new List<Modifier>();

        public override ModifierEffect Copy()
        {
            return new ModifierEffect
            {
                duration = duration,
                targetAttribute = targetAttribute,
                modifierSourceString = modifierSourceString,
                modifiers = modifiers.Select(x => x.Copy()).ToList(),
            };
        }

        public override bool AppliedTo(Character character)
        {
            base.AppliedTo(character);
            if (!character.TryGetAttr(targetAttribute, out var attr)) return false;
            foreach (var modifier in modifiers)
            {
                var copy = modifier.Copy();
                attr.AddModifier(copy);
                created.Add(copy);
            }

            return true;
        }

        public override void Removed()
        {
            if (!target.TryGetAttr(targetAttribute, out var attr)) return;
            foreach (var mod in created)
            {
                attr.RemoveModifier(mod);
            }
        }
    }
}