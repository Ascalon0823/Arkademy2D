using System;
using System.Collections.Generic;
using Arkademy.CharacterCreation;
using Arkademy.Common;
using Arkademy.Gameplay.Ability;
using Arkademy.UI;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Character : MonoBehaviour
    {
        public Common.Character characterData;
        public CharacterGraphic graphic;
        public Rigidbody2D rb;
        public Collider2D hitBox;
        public bool isDead;
        public bool showDamageText;
        public bool indestructible;
        public bool invincible;
        public int faction;
        public Vector2 move;
        public Vector2 facing;
        public bool moving;
        public List<AbilityBase> abilities = new ();
        public InteractableDetector interactableDetector;
        public float playerMoveMultiplier = 1f;
        public static Character Create(Common.Character data, int newFaction)
        {
            var raceName = data.raceName;
            var raceSO = Resources.Load<Race>(raceName);
            if (!raceSO || !raceSO.behaviourPrefab) return null;
            var go = Instantiate(raceSO.behaviourPrefab);
            go.characterData = data;
            go.graphic.animator.runtimeAnimatorController = raceSO.animationController;
            go.graphic.facingLeft = !raceSO.facingRight;
            go.graphic.walkAnimationDistance = 4;
            go.faction = newFaction;
            go.graphic.character = go;
            return go;
        }

        private void Start()
        {
            if (hitBox)
            {
                hitBox.RegisterCharacterCollider(this);
            }
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        public void HandleMovement()
        {
            if (isDead) return;
            var velocity = GetMoveSpeed() * move;
            moving = velocity.sqrMagnitude > 0f;
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        
        public void Move(Vector2 dir)
        {
            move = dir * playerMoveMultiplier;
            if (move.sqrMagnitude > 0)
            {
                facing = move;
            }
        }

        public float GetMoveSpeed()
        {
            return Calculation.MoveSpeed(characterData.speed.value);
        }

        public void SetDead()
        {
            graphic.SetDead();
            isDead = true;
            hitBox.gameObject.SetActive(false);
        }

        public void SetAttack(float cooldown = 1f)
        {
            graphic.attackSpeed = 1f/cooldown;
            graphic.SetAttack();
        }
        

        public void TakeDamage(int damage)
        {
            if (invincible) return;

            graphic.SetHit();
            if (showDamageText)
            {
                DamageCanvas.AddTextTo(Player.Camera, transform, damage.ToString());
            }

            if (indestructible) return;
            characterData.health.currValue -= damage;
            if (characterData.health.currValue <= 0)
            {
                characterData.health.currValue = 0;
                SetDead();
            }
        }
    }
}