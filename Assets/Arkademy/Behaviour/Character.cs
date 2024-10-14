using System;
using Arkademy.Templates.ScriptableObjects;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Character : MonoBehaviour
    {
        public Data.Character data;
        public CharacterGraphic graphic;
        public PhysicsMotor motor;

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
        }

        public void MoveDir(Vector2 dir)
        {
            if (!motor) return;
            motor.moveDir = dir;
            if (!graphic) return;
            graphic.walkSpeed = motor.speed;
            graphic.facing = dir.normalized;
            graphic.moveDir = dir.normalized;
        }

        private void Update()
        {
            UpdateComponentsData();
        }

        private void UpdateComponentsData()
        {
            if (motor && data.TryGetAttribute("MoveSpeed", out var speed, out var allocatedSpeed) )
            {
                motor.speed = (speed.value + allocatedSpeed.value) / 100.0f;
            }
        }
    }
}