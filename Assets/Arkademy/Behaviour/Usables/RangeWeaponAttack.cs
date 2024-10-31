using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Arkademy.Templates;
using UnityEngine;

namespace Arkademy.Behaviour.Usables
{
    public class RangeWeaponAttack : EquipmentAbility
    {
        public Ammunition currAmmu;
        public float delayPercentage;
        public Character target;

        public override bool CanUse()
        {
            return base.CanUse() && currAmmu.stack > 0 && GetTarget();
        }

        public Character GetTarget()
        {
            if (target && user.ValidTarget(target)) return target;
            target = FindObjectsByType<Character>(FindObjectsSortMode.None)
                .Where(x => user.ValidTarget(x))
                .OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault();
            return target;
        }

        IEnumerator InternalUse()
        {
            yield return new WaitForSeconds(delayPercentage * nextUseTime);
            var template = Resources.Load<AmmunitionTemplate>(currAmmu.templateName);
            var projectile = template.GetAmmuBehaviour();
            projectile.remainingLife = 1f;
            var damages = currAmmu.damagePercentage.damages.Select(x =>
            {
                var offen = new Formula.OffensiveData
                {
                    atk = equipment.data.TryGetAttr("Physical Attack", out var d) ? d.GetValue() : 0,
                    ability = 100,
                    damageBuff = x,
                    statScaling = user.data.TryGetAttr("Dexterity", out var dex) ? dex.GetValue() : 0
                };
                var defense = new Formula.DefensiveData();
                return Formula.CalculateDamage(offen, defense);
            }).ToArray();

            projectile.onHit.AddListener(go =>
            {
                var damageable = go.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.TakeDamage(new Data.DamageEvent { damages = damages });
                }

                projectile.triggerCount--;
                DestroyImmediate(projectile.gameObject);
            });
            var diff = target.transform.position - transform.position;
            var motor = projectile.motor;
            projectile.graphic.sprite = template.uiSprite;
            projectile.triggerCount = 1;
            motor.moveSpeed = Mathf.Max(0.01f, currAmmu.travelSpeed / 100f);
            if (motor is FollowMotor fmotor)
            {
                fmotor.target = target.damageable.transform;
                if (fmotor is FollowMotorFixedTime ffmotor)
                {
                    ffmotor.origin = transform.position;
                    ffmotor.usingTime = Mathf.Min(
                        diff.magnitude / motor.moveSpeed,
                        projectile.remainingLife);
                    ffmotor.UpdatePosition(0);
                }
            }
            
        }

        public override void Use()
        {
            if (!CanUse()) return;
            if (!target) return;
            nextUseTime = useTime;
            if (equipment.data.TryGetAttr("Base Speed", out var spd))
            {
                nextUseTime = 1f / (spd.GetValue() / 100f);
            }
            StartCoroutine(InternalUse());
        }
    }
}