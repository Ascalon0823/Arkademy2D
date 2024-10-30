using System.Collections;
using Arkademy.Behaviour;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class EndlessEnemySpawner : CharacterSpawner
    {
        public float delay;
        public override void Spawn()
        {
            base.Spawn();
            lastSpawnedCharacter.onDeath.AddListener(()=>StartCoroutine(DelaySpawn()));
        }

        IEnumerator DelaySpawn()
        {
            yield return new WaitForSeconds(delay);
            Spawn();
        }
    }

}
