using System.Collections;
using Midterm.Character;
using UnityEngine;

namespace Midterm.Field
{
    public class SummonSlimes : Ability
    {
        public Enemy slimePrefab;
        public override float GetCooldown()
        {
            return base.GetCooldown() + Random.Range(0f, 10f);
        }

        public override void Use()
        {
            base.Use();
            StartCoroutine(SpawnSlimes());
        }

        public IEnumerator SpawnSlimes()
        {
            var count = Random.Range(20, 30);
            for (var i = 0; i < count; i++)
            {
                var slime = Instantiate(slimePrefab,transform.position + (Vector3)Random.insideUnitCircle.normalized * Random.Range(4f,8f)
                , Quaternion.identity);
                yield return new WaitForSeconds(GetUseTime() / count);
            }
        }
    }
}