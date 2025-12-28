using System;
using System.Linq;
using Arkademy2D.Game.Behaviours.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public class AICharacterController : MonoBehaviour
    {
        public Character character;
        public float targetDetectionRange;
        private void Update()
        {
            if (character.movement && character.health)
            {
                MoveTowardsTarget(LookingForTarget());
            }
        }

        public Vector2 LookingForTarget()
        {
            var targets = Physics2D.OverlapCircleAll(transform.position, targetDetectionRange);
            if (targets == null || targets.Length == 0)
            {
                return transform.position;
            }

            var candidate = targets.Select(x => x.GetComponent<Health>())
                .Where(x => x && x.faction != character.health.faction)
                .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
                .FirstOrDefault();
            if (!candidate) return transform.position;
            return candidate.transform.position;
        }

        public void MoveTowardsTarget(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);
            if (distance > float.Epsilon)
            {
                character.movement.moveDir = (target - (Vector2)transform.position).normalized;
            }
        }
    }
}