using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static.Item;
using UnityEngine;
namespace Arkademy2D.Game.Behaviours

{
    public class Character : MonoBehaviour
    {
        public Core.Models.Character Model;
        public Actor.Movement movement;
        public Actor.Graphic graphic;
        public Actor.Health health;
        public Actor.User user;
        public Actor.Energy energy;
        public Actor.Caster caster;
        public Interaction.Detector interactionDetector;
        public List<ItemData> items;
        
        public void SetupUseCharacterData(Core.Models.Character model)
        {
            Model = model;
            ReloadCharacterActor();
        }
        private void ReloadCharacterActor()
        {
            health.onDamage.RemoveAllListeners();
            health.onDamage.AddListener(() => { graphic.SetAnimationTrigger("hit"); });
            health.max = Model.MaxHealth;
            health.current = health.max;
            energy.max = Model.MaxEnergy;
            energy.current = Model.MaxEnergy;
            energy.currentFloat = Model.MaxEnergy;
            movement.speed = Model.MoveSpeed;
            items = Model.Items.Select(x =>
            {
                var baseItem = ItemBase.Library.GetValueOrDefault(x.ItemBaseId);
                var usables = baseItem.usableBindings.Select(y => new UsableData
                {
                    usableBase = y.usableBase,
                    usableEffects = y.usableEffects,
                });
                return new ItemData
                {
                    Model = x,
                    itemBase = baseItem,
                    usableData = usables.ToList()
                };
            }).ToList();
        }

        public void Update()
        {
            foreach (var item in items)
            {
                item.Update(Time.deltaTime);
            }

            movement.enabled = health.current > 0f;
        }
    }
}