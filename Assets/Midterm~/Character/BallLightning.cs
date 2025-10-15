using System.Collections;
using UnityEngine;

namespace Midterm.Character
{
    public class BallLightning : Spell
    {
        [SerializeField] private float originalSpeed;
        [SerializeField] private float interval;
        [SerializeField] private float lastUse;

        public override void BeginUse(Vector2 pos)
        {
            base.BeginUse(pos);
            originalSpeed = user.moveSpeed;
            user.moveSpeed = 3 * originalSpeed;
            user.collider.isTrigger = true;
        }

        public override void Use(Vector2 pos)
        {
            base.Use(pos);
            if (casting)
            {
                if (lastUse == 0 || (Time.timeSinceLevelLoad - lastUse >= interval / user.attackSpeed))
                {
                    var group = Random.Range(-int.MaxValue, int.MaxValue);
                    lastUse = Time.timeSinceLevelLoad;
                    var colliders = Physics2D.OverlapCircleAll(user.body.position, 2f);
                    foreach (var collider in colliders)
                    {
                        var chara = collider.gameObject.GetComponent<Character>();
                        if (!chara || chara == user) continue;
                        StartCoroutine(DoDamage(chara,100,group));
                    }
                }
            }
        }
        public IEnumerator DoDamage(Character chara, int damage,int group)
        {
            for (var i = 0; i < 3; i++)
            {
                chara.TakeDamage(damage,group);
                yield return new WaitForSeconds(0.1f);
            }
        }
        public override void EndUse(Vector2 pos)
        {
            base.EndUse(pos);
            user.collider.isTrigger = false;
            user.moveSpeed = originalSpeed;
        }
    }
}