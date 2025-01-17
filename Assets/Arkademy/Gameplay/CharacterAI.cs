using System;
using System.Linq;
using Arkademy.Gameplay.Ability;
using UnityEngine;

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
            var enemies = FindEnemies();
            target = SelectEnemy(enemies);
            if (!UseAbility())
            {
                Move();
            }
        }

        public Character[] FindEnemies()
        {
            var colliders = Physics2D.OverlapCircleAll(character.transform.position,
                Common.Calculation.DetectionRange(character.characterData.detectionRange.value));
            return colliders.Select(x => x.GetCharacter(out var e) ? e : null)
                .Where(x => x && x.faction != character.faction && !x.isDead)
                .ToArray();
        }

        public Character SelectEnemy(Character[] enemies)
        {
            return enemies.OrderBy(x => Vector3.Distance(x.transform.position, character.transform.position))
                .FirstOrDefault();
        }

        public void Move()
        {
            if (!target || !autoMove) return;
            var dir = target.transform.position - character.transform.position;
            character.Move(dir.normalized);
        }

        public bool UseAbility()
        {
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
                if (ability.CanUse(eventData))
                {
                    ability.Use(eventData);
                    return true;
                }
            }

            return false;
        }
    }
}