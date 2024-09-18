using UnityEngine;

namespace Arkademy.Abilities
{
    public class FireAura : Ability
    {
        public Trigger triggerPrefab;
        public int baseDamage;
        public float interval;
        public Trigger spawned;
        private DamageEvent GetDamageEvent()
        {
            return new DamageEvent
            {
                amount = baseDamage + level,
                batch = 0,
                dealerInstance = instanceId,
            };
        }

        protected override void Use()
        {
            base.Use();
            useCount++;
            spawned =  Instantiate(triggerPrefab, user.transform);
            spawned.gameObject.SetLayerRecursive(gameObject.layer);
            spawned.cooldown = interval;
            spawned.onEnter = true;
            spawned.onTrigger.AddListener(OnTrigger); 
        }

        public void OnTrigger(Collider2D other)
        {
            if (other.gameObject.layer == gameObject.layer) return;
            var chara = other.GetComponent<CharacterBehaviour>();
            if (chara) chara.TakeDamage(GetDamageEvent());
        }

        public override void OnLevelUp()
        {
            base.OnLevelUp();
            spawned.transform.localScale = new Vector3(1 + level / 4, 1 + level / 4, 1);
        }
    }
}