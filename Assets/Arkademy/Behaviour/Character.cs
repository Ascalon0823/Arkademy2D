using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Arkademy.Data;
using Arkademy.Templates;
using Arkademy.UI.Game;
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
        public CircleCollider2D body;
        public Damageable damageable;
        public Destructible destructible;
        public Usable usable;

        private List<ISubscription> _handles = new List<ISubscription>();

        public void Setup(Data.Character newData, int newFaction)
        {
            data = newData;
            name = data.name;
            var template = Resources.Load<CharacterTemplate>(newData.templateName);
            faction = newFaction;

            foreach (var handle in _handles)
            {
                handle.Dispose();
            }

            if (data.TryGetAttr(Data.Character.MSpd, out var speed))
            {
                var rb = gameObject.GetOrAddComponent<Rigidbody2D>();
                rb.freezeRotation = true;
                rb.gravityScale = 0f;
                motor = gameObject.GetOrAddComponent<PhysicsMotor>();
                motor.rb = rb;
                _handles.Add(
                    speed.Subscribe(values => { motor.speed = values / 100f; }));
            }

            if (data.TryGetAttr("Size", out var size))
            {
                body = gameObject.GetOrAddComponent<CircleCollider2D>();
                _handles.Add(size.Subscribe(curr => { body.radius = curr / 200f; }));
            }

            if (data.TryGetAttr(Data.Character.DmgEfc, out var damageEffe))
            {
                damageable ??= new GameObject("DamageReceiverTrigger").AddComponent<Damageable>();
                var damageReceiverTrigger = damageable.GetOrAddComponent<CircleCollider2D>();
                damageReceiverTrigger.gameObject.layer = LayerMask.NameToLayer("Hitbox");
                damageReceiverTrigger.isTrigger = true;
                damageable.transform.SetParent(transform, false);
                damageable.trigger = damageReceiverTrigger;
                damageable.faction = faction;
                damageable.OnDamageEvent = null;
                damageable.OnDamageEvent += OnDamageTaken;
                _handles.Add(damageEffe.Subscribe(curr =>
                {
                    damageable.damageEffectiveness = curr;
                    var radius = body ? body.radius : 0.5f;
                    damageable.trigger.radius = radius * (damageable.faction == 1 ? 0.8f : 1.25f);
                },true));
            }

            if (data.TryGetAttr(Data.Character.Life, out var life))
            {
                destructible = gameObject.GetOrAddComponent<Destructible>();
                destructible.durability = life.GetValue();
                destructible.OnDurabilityUpdated += OnDurabilityUpdated;
                life.Subscribe((curr) => { destructible.SetDurability(curr); });
            }

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

            setupCompleted = true;
        }


        private void Start()
        {
            if (!setupCompleted)
            {
                Setup(data, gameObject.CompareTag("Player") ? 1 : 0);
            }
        }

        private void OnDestroy()
        {
            foreach (var handle in _handles)
            {
                handle.Dispose();
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
            if (Application.isEditor)
            {
                foreach (var handle in _handles)
                {
                    handle.Trigger();
                }
            }
        }

        private void OnDamageTaken(Data.DamageEvent damage)
        {
            Debug.Log("Hit");
            if (graphic)
            {
                graphic.SetHit();
            }

            DamageTextCanvas.AddTextTo(transform, damage);
            if (!data.TryGetAttr(Data.Character.Life, out var life)) return;
            life.Value = life.Value - damage.damages.Sum();
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
    }
}