using System;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Ability
{
    public class MeleeUsage : Usage
    {
        public override void Init(Usable usable)
        {
            base.Init(usable);
            var candidate = Physics2D.OverlapCircleAll(
                    transform.position + (Vector3)usable.user.faceDir, 0.5f)
                .Select(x => x.GetComponent<Character>())
                .Where(x => x && x.faction != usable.user.faction && x.hp > 0)
                .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
                .FirstOrDefault();
            if (candidate)
            {
                candidate.TakeDamage(1000);
            }
            Destroy(gameObject);
        }
    }
}