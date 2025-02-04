using System;
using System.Collections.Generic;
using Arkademy.CharacterCreation;
using Arkademy.Data;
using Arkademy.Gameplay.Ability;
using Arkademy.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Attribute = Arkademy.Data.Attribute;
using Race = Arkademy.Data.Race;

namespace Arkademy.Gameplay
{
    public class Character : MonoBehaviour
    {
        public Data.Character data;
        public CharacterGraphic graphic;
        public Rigidbody2D body;
        public Collider2D hitBox;
        public bool isDead;
        public int faction;
        public Vector2 wantToMove;
        public Vector2 facing;
        public List<AbilityBase> abilities = new();
        public InteractableDetector interactableDetector;
        public UnityEvent<DamageData> OnDeath;

        public static Character Create(Race race, Data.Character data, int newFaction)
        {
            if (race == null) return null;
            var go = Instantiate(race.behaviourPrefab);
            go.SetupAs(race, data, newFaction);
            return go;
        }

        public void SetupAs(Race race, Data.Character newData, int newFaction)
        {
            data = newData;
            graphic.animator.runtimeAnimatorController = race.animationController;
            graphic.facingLeft = race.facingLeft;
            graphic.walkAnimationDistance = 4;
            faction = newFaction;
            graphic.character = this;
        }

        private void Start()
        {
            if (hitBox)
            {
                hitBox.RegisterCharacterCollider(this);
            }
        }

        public bool IsMoving()
        {
            return wantToMove.sqrMagnitude > 0;
        }

        public void TakeDamage(DamageData damage)
        {
            graphic.SetHit();
            data.SetCurr(Attribute.Type.Life, Mathf.Max(0, data.GetBase(Attribute.Type.Life) - damage.amount));
            if (isDead)
            {
                OnDeath?.Invoke(damage);
            }
        }

        private void FixedUpdate()
        {
            HandleDeath();
            HandleMove();
        }

        private void HandleMove()
        {
            var velocity = wantToMove *  data.Get(Attribute.Type.MovSpeed);
            if (velocity.sqrMagnitude > 0)
            {
                facing = velocity.normalized;
            }

            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }

        private void HandleDeath()
        {
            body.simulated = !isDead;
            hitBox.enabled = !isDead;
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
            body.position = pos;
        }

        public void SetAttack(float attackTime = 1f)
        {
            graphic.attackSpeed = 1f / attackTime;
            graphic.SetAttack();
        }
    }
}