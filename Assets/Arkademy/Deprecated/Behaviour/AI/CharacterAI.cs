using System;
using System.Linq;
using UnityEngine;

namespace Arkademy.Behaviour.AI
{
    public class CharacterAI : MonoBehaviour
    {
        public Character target;
        public Character currEnemy;

        private void Update()
        {
            if (!target) return;
            if (!currEnemy)
            {
                currEnemy = FindEnemy();
                if(currEnemy)
                    currEnemy.onDeath.AddListener(() => { currEnemy = null; });
            }

            if (!currEnemy) return;
            if (UseUsable()) return;
            if (TargetInRange()) return;
            target.MoveDir((currEnemy.transform.position - target.transform.position).normalized);
        }

        private Character FindEnemy()
        {
            var nearestEnemy = FindObjectsByType<Character>(FindObjectsSortMode.None).Where(x =>
                    x.destructible && x.destructible.durability > 0 && x.faction != target.faction)
                .OrderBy(x => Vector3.Distance(x.transform.position, target.transform.position)).FirstOrDefault();
            return nearestEnemy;
        }

        private bool TargetInRange()
        {
            if (!currEnemy) return false;
            if (!target.usable) return false;
            return Vector3.Distance(target.transform.position, currEnemy.transform.position) < target.usable.range;
        }

        private bool UseUsable()
        {
            if (!target.usable) return false;
            if (!currEnemy) return false;
            if (Vector3.Distance(currEnemy.transform.position, target.transform.position) >
                target.usable.range) return false;
            return target.Use();
        }
    }
}