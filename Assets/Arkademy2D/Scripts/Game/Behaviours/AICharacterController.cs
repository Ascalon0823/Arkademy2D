using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public class AICharacterController : MonoBehaviour
    {
        public Character character;
        public float targetDetectionRange;

        private void Update()
        {
            if (character)
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

            var candidate = targets.Select(x => x.GetComponent<Character>())
                .Where(x => x && x.faction != character.faction)
                .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
                .FirstOrDefault();
            if (!candidate) return transform.position;
            return candidate.transform.position;
        }

        public void MoveTowardsTarget(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);
            var moveDir = distance > float.Epsilon ? (target - (Vector2)transform.position).normalized : Vector2.zero;
            character.moveDir = moveDir;
        }
    }
}