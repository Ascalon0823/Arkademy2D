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
            var enemies = FindEnemies();
            target = SelectEnemy(enemies);

            if (!UseAbility(out var reach))
            {
                Move(reach);
            }
        }

        public Character[] FindEnemies()
        {
            var colliders = Physics2D.OverlapCircleAll(character.transform.position,
                character.Attributes.Get(Attribute.Type.Vision));
            return colliders.Select(x => x.GetCharacter(out var e) ? e : null)
                .Where(x => x && x.faction != character.faction && !x.isDead)
                .ToArray();
        }

        public Character SelectEnemy(Character[] enemies)
        {
            return enemies.OrderBy(x => Vector3.Distance(x.transform.position, character.transform.position))
                .FirstOrDefault();
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