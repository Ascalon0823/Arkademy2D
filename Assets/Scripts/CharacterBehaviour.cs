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
    [Serializable]
    public struct DamageEvent
    {
        public int dealerInstance;
        public int batch;
        public int amount;
    }

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
        public UnityEvent<CharacterBehaviour> onLevelUp;

        public List<Ability> currentAbilities = new();

        public int nextXp;
        public int nextLevelUpXp;
        public int XP;
        public int level;
        [SerializeField] private int prevXp;

        public float pickupRange;

        public float source;
        public bool canLevelUp;

        protected virtual void Start()
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
                    AddAbility(db.abilityData[idx]);
                }
            }

            XP = 0;
            nextXp = 15;
            nextLevelUpXp = 5;
            prevXp = 0;
        }

        protected virtual void Pickup()
        {
            if (Player.Chara != this) return;
            var colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange, LayerMask.GetMask("Pickup"));
            foreach (var c in colliders)
            {
                var pickup = c.GetComponent<Pickup>();
                if (!pickup) continue;
                if (pickup.pickUpBy) continue;
                pickup.pickUpBy = this;
            }
        }

        protected virtual void Update()
        {
            animator.SetBool(Dead, isDead);
            animator.updateMode = isDead ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
            if (isDead)
            {
                return;
            }

            if (isHit)
            {
                animator.SetTrigger(Hit);
                isHit = false;
            }

            Pickup();
            velocity = moveSpeed * Time.deltaTime * wantToMove;
            rb.MovePosition(rb.position + velocity);
            animator.SetBool(Walking, velocity.magnitude > 0f);
            if (velocity.magnitude > 0f)
            {
                var facing = Vector2.Dot(velocity, Vector2.left);
                sprite.flipX = leftFacing ? facing < 0f : facing > 0f;
            }
        }

        public virtual void TakeDamage(DamageEvent de)
        {
            life -= de.amount;
            if (showDamageNumber)
                DamageTextCanvas.AddTextTo(transform, de);
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

        public CharacterBehaviour[] GetNearestEnemies(int amount)
        {
            return StageBehaviour.Current.spawnedCharacters
                .Where(x => x.gameObject.layer != gameObject.layer)
                .OrderBy(x => Vector2.Distance(
                    x.transform.position, transform.position)).Take(amount).ToArray();
        }

        public void GainXP(int xp)
        {
            XP += xp;

            ResolveLevelUp();
        }

        public void ResolveLevelUp()
        {
            if (XP < nextLevelUpXp) return;
            prevXp = nextLevelUpXp;
            nextLevelUpXp += nextXp;
            nextXp += level > 40 ? 16 : level > 20 ? 13 : 10;
            level++;
            onLevelUp?.Invoke(this);
        }

        public void AddAbility(Database.AbilityData abilityData)
        {
            var ability = Instantiate(abilityData.prefab, transform);
            ability.gameObject.SetLayerRecursive(gameObject.layer);
            ability.abilityName = abilityData.name;
            ability.uiIcon = abilityData.uiIcon;
            currentAbilities.Add(ability);
            ability.user = this;
            ability.level = 1;
        }

        public float GetLifePercent()
        {
            return life * 1.0f / charaData.life;
        }

        public float GetXpPercent()
        {
            return (XP - prevXp) * 1.0f / (nextLevelUpXp - prevXp);
        }

        public void AddOrLevelUpAbility(Database.AbilityData abilityData)
        {
            var charaAbi = currentAbilities.FirstOrDefault(x => x.abilityName == abilityData.name);
            if (charaAbi)
            {
                charaAbi.level += 1;
                charaAbi.OnLevelUp();
                return;
            }

            AddAbility(abilityData);
        }
    }
}