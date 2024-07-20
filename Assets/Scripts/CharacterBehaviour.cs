using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arkademy.UI.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Arkademy
{
    public class CharacterBehaviour : MonoBehaviour
    {
        public Database.CharacterData charaData;
        public bool isDead;
        public bool isHit;
        public Rigidbody2D rb;
        public Collider2D collider2d;
        public Vector2 wantToMove;
        public float moveSpeed;
        public Vector2 velocity;
        public Animator animator;
        public SpriteRenderer sprite;
        public bool leftFacing;
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int Dead = Animator.StringToHash("dead");
        private static readonly int Hit = Animator.StringToHash("hit");

        public int life;

        public bool showDamageNumber;

        public UnityEvent<CharacterBehaviour> onDeath;

        public List<Ability> currentAbilities = new();

        private void Start()
        {
            if (StageBehaviour.Current)
            {
                StageBehaviour.Current.spawnedCharacters.Add(this);
            }

            if (charaData?.beginningAbilityIdx?.Length > 0)
            {
                var db = Database.GetDatabase();
                foreach (var idx in charaData.beginningAbilityIdx)
                {
                    var ability = Instantiate(db.abilityData[idx].prefab,transform);
                    ability.gameObject.SetLayerRecursive(gameObject.layer);
                    currentAbilities.Add(ability);
                    ability.user = this;
                    ability.level = 1;
                }
            }
        }

        protected virtual void Update()
        {
            animator.SetBool(Dead, isDead);
            if (isDead) return;
            if (isHit)
            {
                animator.SetTrigger(Hit);
                isHit = false;
            }

            velocity = moveSpeed * Time.deltaTime * wantToMove;
            rb.MovePosition(rb.position + velocity);
            animator.SetBool(Walking, velocity.magnitude > 0f);
            if (velocity.magnitude > 0f)
            {
                var facing = Vector2.Dot(velocity, Vector2.left);
                sprite.flipX = leftFacing ? facing < 0f : facing > 0f;
            }
        }

        public virtual void TakeDamage(int damage)
        {
            life -= damage;
            if (showDamageNumber)
                DamageTextCanvas.AddTextTo(transform, damage);
            if (life <= 0)
            {
                isDead = true;
                if (StageBehaviour.Current
                    && StageBehaviour.Current.spawnedCharacters.Contains(this))
                    StageBehaviour.Current.spawnedCharacters.Remove(this);
                foreach (var ability in currentAbilities)
                {
                    ability.enabled = false;
                }
                onDeath?.Invoke(this);
                return;
            }

            isHit = true;
        }

        public CharacterBehaviour GetNearestEnemy()
        {
            return StageBehaviour.Current.spawnedCharacters
                .Where(x => x.gameObject.layer != gameObject.layer)
                .OrderBy(x => Vector2.Distance(
                    x.transform.position, transform.position)).FirstOrDefault();
        }
    }
}