using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Midterm.Character
{
    public class Character : MonoBehaviour
    {
        public Rigidbody2D body;
        public new CircleCollider2D collider;
        public Vector2 moveDir;
        public Vector2 faceDir;
        public float moveSpeed;
        public float attackSpeed;
        public float power;

        public UnityEvent onDead;

        public bool preparing;

        public void SetupAs(PlayableCharacterData data)
        {
            gameObject.name = data.internalName;
            moveSpeed = data.speed;
            attackSpeed = data.atkSpeed;
            animator.runtimeAnimatorController = data.animator;
            life = data.life;
            maxLife = data.life;
            power = data.power;
            abilities.Clear();
            foreach (var ability in data.initialAbilities)
            {
                var abilityInstance = Instantiate(ability, transform);
                abilities.Add(abilityInstance);
                abilityInstance.user = this;
            }
        }

        private void Update()
        {
            UseAbilities();
            Cast();
        }

        private void LateUpdate()
        {
            UpdateGraphic();
        }

        private void FixedUpdate()
        {
            collider.enabled = life > 0;
            Move();
        }


        public void Move()
        {
            if (life <= 0)
            {
                return;
            }

            if (preparing) return;
            body.MovePosition(body.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
            if (moveDir.magnitude > 0.01f)
            {
                faceDir = moveDir.normalized;
            }
        }

        public Animator animator;
        public SpriteRenderer spriteRenderer;

        public void UpdateGraphic()
        {
            var isMoving = moveDir.magnitude > 0.01f;
            animator.SetBool("walking", isMoving);
            animator.SetFloat("walkSpeed", moveSpeed / 4f);
            spriteRenderer.flipX = faceDir.x > 0;
            if (life <= 0)
            {
                if (!animator.GetBool("dead"))
                    animator.SetBool("dead", true);
            }
            else
            {
                animator.SetBool("dead", false);
            }
        }

        public int life;
        public int maxLife;

        public void TakeDamage(int damage)
        {
            life -= damage;
            life = Mathf.Clamp(life, 0, maxLife);
            if (life <= 0)
            {
                onDead?.Invoke();
            }

            animator.SetTrigger("hit");
        }

        public List<Ability> abilities = new List<Ability>();

        public void UseAbilities()
        {
            if (life <= 0) return;
            foreach (var ability in abilities)
            {
                if (ability.CanUse())
                {
                    ability.Use();
                }
            }
        }

        public void LevelUpAbility(Ability ability)
        {
            var existing = abilities.FirstOrDefault(x => x.internalName == ability.internalName);
            if (existing)
            {
                existing.currLevel++;
                return;
            }

            var newAbi = Instantiate(ability, transform);
            abilities.Add(newAbi);
            newAbi.user = this;
        }

        public Spell currSpell;

        public List<Spell> spells = new List<Spell>();
        public float energy;
        public int maxEnergy;
        public Vector2 pointAt;
        public bool casting;
        public bool wasCasting;

        public void ChangeSpell(string spellKey)
        {
            if (currSpell && currSpell.casting) return;
            if (currSpell && currSpell.key == spellKey) return;
            if (string.IsNullOrEmpty(spellKey)) return;
            var newSpellPrefab = spells.FirstOrDefault(x => x.key == spellKey);
            if (!newSpellPrefab) return;
            if (currSpell)
            {
                Destroy(currSpell.gameObject);
            }

            currSpell = Instantiate(newSpellPrefab, transform);
            currSpell.user = this;
        }

        public void Cast()
        {
            if (!currSpell) return;
            InternalCast();
            wasCasting = casting;
        }

        private void InternalCast()
        {
            if (wasCasting && !casting)
            {
                currSpell.EndUse(pointAt);
                return;
            }

            if (!wasCasting && casting)
            {
                currSpell.BeginUse(pointAt);
                return;
            }

            if (wasCasting && casting)
            {
                currSpell.Use(pointAt);
            }
        }

        public void GainEnergy(int amount)
        {
            energy += amount;
            energy = Mathf.Clamp(energy, 0, maxEnergy);
        }
    }
}