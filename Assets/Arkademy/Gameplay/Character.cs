﻿using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.CharacterCreation;
using Arkademy.Data;
using Arkademy.Gameplay.Ability;
using Arkademy.Gameplay.Pickup;
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
        public Vector2 velocity;
        public Vector2 facing;
        public List<AbilityBase> abilities = new();
        public InteractableDetector interactableDetector;
        public UnityEvent<DamageData> OnDeath;
        public bool setupCompleted;

        public Attributes Attributes;

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
            Attributes = new Attributes(race, data);
            graphic.animator.runtimeAnimatorController = race.animationController;
            graphic.facingLeft = race.facingLeft;
            graphic.walkAnimationDistance = 4;
            faction = newFaction;
            graphic.character = this;
            foreach (var abilityName in race.abilities)
            {
                var ability = Data.Ability.GetAbility(abilityName.abilityName);
                var level = data.GetAbilityLevel(abilityName.abilityName);
                if (level == 0) continue;
                var instance = AbilityBase.CreateAbility(ability);
                instance.GiveToUser(this);
            }

            foreach (var slot in data.equipmentSlots)
            {
                if (slot.equipment != null)
                {
                    slot.equipment.providedAbilities.Clear();
                    ChangeEquipment(slot.equipment);
                }
            }

            setupCompleted = true;
        }

        private void Start()
        {
            if (!setupCompleted)
            {
                SetupAs(Race.GetRace(data.raceName), data, faction);
            }

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
            var life = Attributes[Attribute.Type.Life];
            life.current = Math.Max(0, life.current - damage.Sum());
            isDead = life.current == 0;
            if (isDead)
            {
                OnDeath?.Invoke(damage);
            }


            DamageCanvas.AddTextTo(Player.Camera, transform, damage.amounts.Select(x =>
                    $"<color=#{ColorUtility.ToHtmlStringRGBA(Color.white)}> {Mathf.CeilToInt(x / 100f).ToString()}</color>")
                .ToArray()
            );
        }

        public void Heal(int amount)
        {
            var life = Attributes[Attribute.Type.Life];
            life.current = Math.Min(life.BaseValue(), life.current + amount);
            DamageCanvas.AddTextTo(Player.Camera, transform,
                $"<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}> {Mathf.CeilToInt(amount / 100f).ToString()}</color>");
        }

        public void KnockBack(Vector2 displacement)
        {
            SetPosition(body.position + displacement);
        }


        private void FixedUpdate()
        {
            HandleDeath();
            HandleMove();
            HandlePickup();
        }

        private void HandlePickup()
        {
            var range = Attributes.Get(Attribute.Type.PickupRange);
            if (range == 0f) return;
            var pickups = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Pickup"));
            if (pickups == null || pickups.Length == 0) return;
            foreach (var pickup in pickups)
            {
                var behaviour = pickup.GetComponent<PickupBase>();
                if (!behaviour) return;
                behaviour.PickupBy(this);
            }
        }

        private void HandleMove()
        {
            velocity = wantToMove * Attributes.Get(Attribute.Type.MovSpeed);
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

        public void ChangeEquipment(Equipment equipment)
        {
            var baseItem = ItemBase.GetItemBase(equipment.baseName);
            if (baseItem == null || !baseItem.isEquipment) return;
            var slot = data.equipmentSlots.FirstOrDefault(x => x.slot == baseItem.slot);
            if (slot == null) return;
            data.AddToInventory(slot.equipment);
            foreach (var ability in slot.equipment.providedAbilities)
            {
                abilities.Remove(ability);
                Destroy(ability.gameObject);
            }

            slot.equipment.providedAbilities.Clear();
            slot.equipment = equipment;
            // var weaponAbility = Instantiate(baseItem.baseAttack,transform);
            // weaponAbility.payloadPrefab = baseItem.abilityPayload;
            // slot.equipment.providedAbilities.Add(weaponAbility);
            // weaponAbility.GiveToUser(this);
            Attributes.UpdateEquipment(slot);
        }

        public List<Character> VisibleCharacters()
        {
            return Physics2D.OverlapCircleAll(transform.position, Attributes.Get(Attribute.Type.Vision))
                .Select(x => x.GetCharacter(out var c) ? c : null)
                .Where(x => x && x.faction != faction && !x.isDead)
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                .ToList();
        }
    }
}