using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Templates;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Character : MonoBehaviour
    {
        public Data.Character data;
        public CharacterGraphic graphic;
        public PhysicsMotor motor;
        public List<EquipmentSlot> equipmentSlots = new();
        public bool setupCompleted;
        public float remainUseTime;

        public void Setup(Data.Character newData)
        {
            data = newData;
            name = data.name;
            var template = Resources.Load<CharacterTemplate>(newData.templateName);
            if (template.animationController != null)
            {
                graphic = Instantiate(Resources.Load<CharacterGraphic>("CharacterGraphic"), transform);
                graphic.transform.localPosition = Vector3.zero;
                graphic.animator.runtimeAnimatorController = template.animationController;
                graphic.facingLeft = !template.facingLeft;
                graphic.walkSpeed = 1;
                graphic.walkAnimationDistance =
                    template.walkAnimationDistance.Equals(0) ? 1 : template.walkAnimationDistance;
                graphic.attackSpeed = 1;
            }

            if (data.TryGetAttribute("MoveSpeed", out var speed, out var allocatedSpeed))
            {
                var rb = gameObject.AddComponent<Rigidbody2D>();
                rb.freezeRotation = true;
                rb.gravityScale = 0f;
                motor = gameObject.AddComponent<PhysicsMotor>();
                motor.rb = rb;
                motor.speed = (speed.value + allocatedSpeed.value) / 100.0f;
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
            if (equipmentSlots == null) return false;
            if (remainUseTime > 0) return false;
            var idx = equipmentSlots.FindIndex(x => x.data.category == Data.EquipmentSlot.Category.MainHand);
            if (idx == -1) return false;
            var mainHand = equipmentSlots[idx];
            if (!mainHand || !mainHand.equipment || mainHand.equipment.data==null) return false;
            var equipmentData = mainHand.equipment.data;
            if (equipmentData.attributes == null) return false;
            var speedIdx = equipmentData.attributes.FindIndex(x => x.key == "Base Speed");
            var speed = speedIdx == -1 ? 100 : equipmentData.attributes[speedIdx].value;
            remainUseTime = Mathf.Max(Time.fixedDeltaTime, 1f / (speed / 100f));
            if (graphic)
            {
                graphic.attackSpeed = 1f/remainUseTime;
                graphic.SetAttack();
            }
            return true;
        }

        private void Update()
        {
            if (remainUseTime > 0) remainUseTime -= Time.deltaTime;
            UpdateComponentsData();
        }

        private void UpdateComponentsData()
        {
            if (motor && data.TryGetAttribute("MoveSpeed", out var speed, out var allocatedSpeed))
            {
                motor.speed = (speed.value + allocatedSpeed.value) / 100.0f;
            }

            for (var i = 0; i < equipmentSlots.Count; i++)
            {
                var slotData = data.slots[i];
                var slot = equipmentSlots[i];
                slotData.equipment = slot.equipment ? equipmentSlots[i].equipment.data : null;
                data.slots[i] = slotData;
            }
        }
    }
}