using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Arkademy.Templates;
using Arkademy.UI.Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Character : MonoBehaviour
    {
        public int faction;
        public Data.Character data;
        public CharacterGraphic graphic;
        public PhysicsMotor motor;
        public List<EquipmentSlot> equipmentSlots = new();
        public bool setupCompleted;
        public float remainUseTime;
        public CircleCollider2D body;
        public Damageable damageable;
        public Destructible destructible;
        public Usable usable;

        public void Setup(Data.Character newData, int newFaction)
        {
            data = newData;
            name = data.name;
            var template = Resources.Load<CharacterTemplate>(newData.templateName);
            faction = newFaction;
            if (template.animationController != null)
            {
                if (!graphic)
                    graphic = Instantiate(Resources.Load<CharacterGraphic>("CharacterGraphic"), transform);
                graphic.transform.localPosition = Vector3.zero;
                graphic.animator.runtimeAnimatorController = template.animationController;
                graphic.facingLeft = !template.facingRight;
                graphic.walkSpeed = 1;
                graphic.walkAnimationDistance =
                    template.walkAnimationDistance.Equals(0) ? 1 : template.walkAnimationDistance;
                graphic.attackSpeed = 1;
            }

            if (data.slots != null)
            {
                foreach (var slotData in data.slots)
                {
                    var slot = gameObject.AddComponent<EquipmentSlot>();
                    slot.Setup(slotData, this);
                    equipmentSlots.Add(slot);
                }
            }

            UpdateComponentsData();
            setupCompleted = true;
        }

        private void Start()
        {
            if (!setupCompleted)
            {
                Setup(data, gameObject.CompareTag("Player") ? 1 : 0);
            }
        }

        public void MoveDir(Vector2 dir)
        {
            if (!motor) return;
            motor.moveDir = dir.normalized;
            if (equipmentSlots != null)
            {
                foreach (var slot in equipmentSlots)
                {
                    if (slot && slot.equipment && !dir.sqrMagnitude.Equals(0))
                        slot.equipment.facingDir = dir.normalized;
                }
            }

            if (!graphic) return;
            graphic.walkSpeed = motor.speed;
            if (!dir.sqrMagnitude.Equals(0)) graphic.facing = dir.normalized;
            graphic.moveDir = dir.normalized;
        }

        public bool Use()
        {
            if (!usable) return false;

            if (graphic && usable.Use())
            {
                graphic.attackSpeed = 1f / usable.nextUseTime;
                graphic.SetAttack();
            }

            return false;
        }

        private void Update()
        {
            if (remainUseTime > 0) remainUseTime -= Time.deltaTime;
            UpdateComponentsData();
        }

        private void BodyAddOrUpdateComponent()
        {
            if (!data.TryGetAttribute("Size", out var size, out _)) return;
            if (!body)
            {
                body = gameObject.AddComponent<CircleCollider2D>();
            }

            if (body)
            {
                body.radius = body.radius = size.value / 200f;
            }
        }

        private void MotorAddOrUpdateComponent()
        {
            if (!data.TryGetAttribute("MoveSpeed", out var speed, out var allocatedSpeed)) return;
            if (!motor)
            {
                var rb = gameObject.AddComponent<Rigidbody2D>();
                rb.freezeRotation = true;
                rb.gravityScale = 0f;
                motor = gameObject.AddComponent<PhysicsMotor>();
                motor.rb = rb;
            }

            if (motor)
            {
                motor.speed = (speed.value + allocatedSpeed.value) / 100.0f;
            }
        }

        private void DamageableAddOrUpdateComponent()
        {
            if (!data.TryGetAttribute("DamageEffectiveness", out var damage, out var allocated)) return;
            if (!damageable)
            {
                damageable = new GameObject("DamageReceiverTrigger").AddComponent<Damageable>();
                var damageReceiverTrigger = damageable.AddComponent<CircleCollider2D>();
                damageReceiverTrigger.gameObject.layer = LayerMask.NameToLayer("Hitbox");
                damageReceiverTrigger.isTrigger = true;
                damageable.transform.SetParent(transform, false);
                damageable.trigger = damageReceiverTrigger;
                damageable.faction = faction;
                damageable.OnDamageEvent += OnDamageTaken;
            }

            if (damageable)
            {
                damageable.damageEffectiveness = damage.value;
                var radius = body ? body.radius : 0.5f;
                damageable.trigger.radius = radius * (damageable.faction == 1 ? 0.8f : 1.25f);
            }
        }

        private void OnDamageTaken(Data.DamageEvent damage)
        {
            Debug.Log("Hit");
            if (graphic)
            {
                graphic.SetHit();
            }
            DamageTextCanvas.AddTextTo(transform,damage);
            if (!data.TryGetAttribute("Life", out var life, out var allocated)) return;
            data.TryUpdateAttribute("Life", life.value - damage.damages.Sum());
            DestructibleAddOrUpdateComponent();
        }

        private void OnDurabilityUpdated(long prev, long curr)
        {
            if (curr <= 0)
            {
                Debug.Log("Dead");
                if (graphic)
                {
                    graphic.SetDead();
                }
            }
        }

        private void DestructibleAddOrUpdateComponent()
        {
            if (!data.TryGetAttribute("Life", out var life, out var allocated)) return;
            if (!destructible)
            {
                destructible = gameObject.AddComponent<Destructible>();
                destructible.durability = life.value;
                destructible.OnDurabilityUpdated += OnDurabilityUpdated;
            }

            if (destructible)
            {
                destructible.SetDurability(life.value);
            }
        }

        private void UpdateComponentsData()
        {
            MotorAddOrUpdateComponent();
            BodyAddOrUpdateComponent();
            DamageableAddOrUpdateComponent();
            DestructibleAddOrUpdateComponent();
            // for (var i = 0; i < equipmentSlots.Count; i++)
            // {
            //     var slotData = data.slots[i];
            //     var slot = equipmentSlots[i];
            //     slotData.equipment = slot.equipment ? equipmentSlots[i].equipment.data : null;
            //     data.slots[i] = slotData;
            // }
        }
    }
}