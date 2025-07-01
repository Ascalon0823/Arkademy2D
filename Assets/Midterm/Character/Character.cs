using System;
using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
            if (indicator) indicator.transform.position = pointAt;
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

        public AudioSource audioSource;
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

        public DamageTextSpawner damageTextSpawner;

        public AudioClip[] hitClips;
        public AudioClip[] deathClips;
        public void TakeDamage(int damage, int group = -1)
        {
            life -= damage;
            life = Mathf.Clamp(life, 0, maxLife);
            if (Player.Player.Local.currCharacter == this)
            {
                Player.Player.Local.shake.Shake(0.25f, 0.2f);
            }

            if (hitClips!=null && hitClips.Length>0)
            {
                audioSource.clip = hitClips[Random.Range(0, hitClips.Length)];
                audioSource.Play();
            }
            if (life <= 0)
            {
                onDead?.Invoke();
                if (deathClips != null && deathClips.Length > 0)
                {
                    audioSource.clip = deathClips[Random.Range(0, deathClips.Length)];
                    audioSource.Play();
                }
               
            }

            spriteRenderer.color = Color.red;
            LeanTween.color(spriteRenderer.gameObject, Color.white, 0.666f).setEaseOutExpo();
            animator.SetTrigger("hit");
            if (damageTextSpawner)
            {
                damageTextSpawner.SpawnDamage(damage,group);
            }
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

        public void LevelUpAbility(Ability ability, Ability.Upgrade upgrade)
        {
            var existing = abilities.FirstOrDefault(x => x.internalName == ability.internalName);
            Debug.Log(ability.internalName);
            if (existing)
            {
                Debug.Log(upgrade);
                upgrade.currLevel++;
                if (ability.levelUpSounds!=null && ability.levelUpSounds.Length>0)
                {
                    audioSource.clip = ability.levelUpSounds[Random.Range(0, ability.levelUpSounds.Length)];
                    audioSource.Play();
                }
                return;
            }

            var newAbi = Instantiate(ability, transform);
            abilities.Add(newAbi);
            newAbi.user = this;
            if (newAbi.equipSound && newAbi.AudioSource)
            {
                audioSource.clip = newAbi.equipSound;
                audioSource.Play();
            }
        }

        public Spell currSpell;

        public List<Spell> spells = new List<Spell>();
        public float energy;
        public int maxEnergy;
        public Vector2 pointAt;
        public bool casting;
        public bool wasCasting;
        public Transform indicator;

        public void ChangeSpell(string spellKey)
        {
            if (!energy.Equals(maxEnergy)) return;
            if (currSpell && currSpell.casting) return;
            if (string.IsNullOrEmpty(spellKey)) return;
            var newSpellPrefab = spells.FirstOrDefault(x => x.key == spellKey);
            if (!newSpellPrefab) return;
            if (currSpell)
            {
                Destroy(currSpell.gameObject);
            }

            currSpell = Instantiate(newSpellPrefab, transform);
            currSpell.user = this;
            currSpell.BeginUse(pointAt);
            FieldManager.Instance?.Darken(true);
            Player.Player.Local.shake.Shake(0.15f, 2f);
        }

        public void Cast()
        {
            if (!currSpell) return;
            if (!currSpell.casting)
            {
                Destroy(currSpell.gameObject);
                FieldManager.Instance?.Darken(false);
                return;
            }

            currSpell.Use(pointAt);
        }

        public void GainEnergy(int amount)
        {
            energy += amount;
            energy = Mathf.Clamp(energy, 0, maxEnergy);
        }
    }
}