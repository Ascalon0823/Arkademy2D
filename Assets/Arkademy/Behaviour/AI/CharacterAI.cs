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
            var nearestEnemy = GameObject.FindObjectsOfType<Character>().Where(x => x.faction != target.faction)
                .OrderBy(x => Vector3.Distance(x.transform.position, target.transform.position)).FirstOrDefault();
            return nearestEnemy;
        }
    }
}
