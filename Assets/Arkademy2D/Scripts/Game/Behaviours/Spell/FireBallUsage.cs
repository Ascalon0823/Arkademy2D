using System;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Spell
{
    public class FireBallUsage : Usage
    {
        public float life;
        public Rigidbody2D body;
        public float speed;
        public override void Init(Usable usable)
        {
            base.Init(usable);
            var target = Physics2D.OverlapCircleAll(usable.transform.position, 30)
                .Select(x => x.gameObject.GetComponent<Character>())
                .Where(x => x && x.faction != usable.user.faction && x.hp>0)
                .OrderBy(x => Vector2.Distance(x.transform.position, usable.transform.position))
                .FirstOrDefault();
            var dir = target? (Vector2)(target.transform.position - usable.transform.position): usable.user.faceDir;
            transform.up = dir;
        }

        private void FixedUpdate()
        {
            life -= Time.fixedDeltaTime;
            if (life <= 0)
            {
                Destroy(gameObject);
                return;
            }
            body.MovePosition(body.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger) return;
            var chara = other.GetComponent<Character>();
            if (chara && chara.faction == fromUsable.user.faction)
            {
                return;
            }
            Destroy(gameObject);
            if (chara)
            {
                chara.TakeDamage(10000);
            }
        }
    }
}