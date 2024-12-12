using System;
using System.Collections.Generic;
using Arkademy.Behaviour;
using Arkademy.Behaviour.UI;
using Arkademy.CharacterCreation;
using Arkademy.Common;
using Arkademy.UI;
using Arkademy.UI.Game;
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

        public static Character Create(Common.Character data)
        {
            var raceName = data.raceName;
            var raceSO = Resources.Load<Race>(raceName);
            if (!raceSO || !raceSO.behaviourPrefab) return null;
            var go = Instantiate(raceSO.behaviourPrefab);
            go.characterData = data;
            go.graphic.animator.runtimeAnimatorController = raceSO.animationController;
            go.graphic.facingLeft = !raceSO.facingRight;
            go.graphic.walkAnimationDistance = 4;
            if (go.hitBox)
            {
                go.hitBox.RegisterCharacterCollider(go);
            }

            return go;
        }

        public void Move(Vector2 dir)
        {
            if (isDead) return;
            var speed = Calculation.MoveSpeed(characterData.speed.value);
            graphic.moveDir = dir;
            graphic.walkSpeed = speed;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
            if (dir.magnitude > 0f)
            {
                graphic.facing = graphic.moveDir;
            }
        }

        public void SetDead()
        {
            graphic.SetDead();
            isDead = true;
            hitBox.gameObject.SetActive(false);
        }

        public void TakeDamage(int damage)
        {
            characterData.health.currValue -= damage;
            graphic.SetHit();
            if (showDamageText)
            {
                DamageCanvas.AddTextTo(Player.Camera, transform, damage.ToString());
            }
            if (characterData.health.currValue <= 0)
            {
                characterData.health.currValue = 0;
                SetDead();
            }
        }
    }
}