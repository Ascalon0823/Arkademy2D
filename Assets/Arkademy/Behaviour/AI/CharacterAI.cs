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
            }

            if (!currEnemy) return;
            target.MoveDir((currEnemy.transform.position - target.transform.position).normalized);
        }

        private Character FindEnemy()
        {
            var nearestEnemy = FindObjectsByType<Character>(FindObjectsSortMode.None).Where(x =>
                    x.destructible && x.destructible.durability > 0 && x.faction != target.faction)
                .OrderBy(x => Vector3.Distance(x.transform.position, target.transform.position)).FirstOrDefault();
            return nearestEnemy;
        }
    }
}