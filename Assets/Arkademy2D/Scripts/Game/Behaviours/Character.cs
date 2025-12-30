using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static.Item;
using UnityEngine;
using Time = UnityEngine.Time;

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
        public Collider2D collision;
        public Damage.Contact contactDamage;
        public Interaction.Detector interactionDetector;
        public List<ItemData> items;
        public Attributes attributes;

        private void Start()
        {
            collision = GetComponent<Collider2D>();
        }

        public void SetupUseCharacterData(Core.Models.Character model)
        {
            Model = model;
            ReloadCharacterActor();
        }
        private void ReloadCharacterActor()
        {
            attributes = new Attributes();
            foreach (var attribute in Model.Attributes)
            {
                attributes.Add(attribute.Key, attribute.Value);
            }
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
            if(movement)
                movement.enabled = health.current > 0f;
            if(contactDamage)
                contactDamage.enabled = health.current > 0f;
            collision.isTrigger = health.current == 0;
        }
    }
}