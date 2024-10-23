using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Behaviour.Usables
{
    public class WeaponSwing : Usable
    {
        public Equipment equipment;

        public float delayPercentage;
        public float effectiveTimePercentage;
        public int[] damagePercentages;

        public DamageDealer dealer;
        public VFXGraphic graphic;

        protected void Awake()
        {
            dealer.gameObject.SetActive(false);
            dealer.beforeDamageEvent.AddListener(CalculateDamageEvent);
        }

        public override bool Use()
        {
            if (!base.Use()) return false;
            dealer.faction = user.faction;
            var cd = equipment.data.attributes.FirstOrDefault(x => x.key == "Base Speed");
            nextUseTime = 1f / (cd.value / 100f);
            StartCoroutine(InternalUse());
            return true;
        }

        public void CalculateDamageEvent(DamageDealer.BeforeDamageEvent d)
        {
            var damage = equipment.data.attributes.FirstOrDefault(x => x.key == "Physical Attack");
            var damageEventBase = new Data.DamageEvent
            {
                damages = new long [damagePercentages.Length]
            };
            var strBoost = user.data.growth.TryGet(Data.Character.Str, out var str) ? str.Value : 0;
            for (var i = 0; i < damagePercentages.Length; i++)
            {
                damageEventBase.damages[i] =
                    Mathf.FloorToInt(Random.Range(90f, 100f) / 100f * damage.value * damagePercentages[i] / 100f * (1+strBoost/100f));
            }

            d.dealer.damageEventBase = damageEventBase;
        }
        IEnumerator InternalUse()
        {
            yield return new WaitForSeconds(delayPercentage * nextUseTime);
            graphic.playSpeed = effectiveTimePercentage * nextUseTime;
            var pos = (Vector2)equipment.transform.position + equipment.facingDir.normalized;
            var size = Vector2.one * 2f;
            dealer.transform.position = pos;
            dealer.transform.up = equipment.facingDir;
            dealer.transform.localScale = size;
            dealer.gameObject.SetActive(true);
            yield return new WaitForSeconds(effectiveTimePercentage * nextUseTime);
            dealer.gameObject.SetActive(false);
        }
    }
}