using System;
using System.Linq;
using Arkademy.Gameplay.Ability;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay
{
    public class CharacterAI : MonoBehaviour
    {
        public bool autoUseAbility;
        public bool autoMove;
        public Character character;
        public Character target;

        public void Update()
        {
            if (character.isDead) return;
            target = character.VisibleCharacters().FirstOrDefault();

            if (!UseAbility(out var reach))
            {
                Move(reach);
            }
        }

        public void Move(bool reach)
        {
            if (!target || !autoMove) return;
            if (reach)
            {
                character.wantToMove = Vector2.zero;
                return;
            }

            var dir = target.transform.position - character.transform.position;
            character.wantToMove = dir.normalized;
        }

        public bool UseAbility(out bool canReach)
        {
            canReach = false;
            if (!autoUseAbility) return false;
            var eventData = new AbilityEventData();
            if (target)
            {
                eventData.PrimaryTarget = target;
                eventData.Direction = (target.transform.position - character.transform.position).normalized;
                eventData.Position = target.transform.position;
            }

            foreach (var ability in character.abilities)
            {
                if (ability.CanReach(eventData))
                {
                    canReach = true;
                }

                if (ability.CanUse(eventData) && canReach)
                {
                    ability.Use(eventData);
                    return true;
                }
            }

            return false;
        }
    }
}