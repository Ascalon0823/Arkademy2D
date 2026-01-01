using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Behaviours.Actor;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Data.Static.Item;
using UnityEngine;
using Attribute = Arkademy2D.Game.Data.Runtime.Attribute;
using Time = UnityEngine.Time;

namespace Arkademy2D.Game.Behaviours

{
    public class Character : MonoBehaviour
    {
        public Actor.Movement movement;
        public Actor.Graphic graphic;
        public Actor.Health health;
        public Actor.Energy energy;
        public Actor.Caster caster;
        public Collider2D collision;
        public Damage.Contact contactDamage;
        public Interaction.Detector interactionDetector;
        public List<ItemData> items;
        public List<Attribute> attributes;
        public List<Usable> usables;

        private void Start()
        {
            collision = GetComponent<Collider2D>();
        }

        public void SetupUseCharacterData(Core.Models.Character model)
        {
            var characterBase = CharacterBase.Library.GetValueOrDefault(model.CharacterBaseId);
            if (!characterBase)
            {
                Debug.LogError($"CharacterBase {model.CharacterBaseId} does not exist");
                return;
            }
            attributes = new List<Attribute>();
            foreach (var attribute in characterBase.attributeConfigs)
            {
                attributes.Add(new Attribute { config =  attribute });
            }
            items = model.Items.Select(x =>
            {
                var baseItem = ItemBase.Library.GetValueOrDefault(x.ItemBaseId);
                
                return new ItemData
                {
                    Model = x,
                    itemBase = baseItem,
                    attributes = baseItem.attributesConfigs.Select(y=>new Attribute{config = y}).ToList()
                };
            }).ToList();
            usables = items.SelectMany(x =>
            {
                if (x.itemBase.usableDefinitions != null && x.itemBase.usableDefinitions.Count > 0)
                {
                    return x.itemBase.usableDefinitions.Select(y =>
                    {
                        var usable = new Usable(y);
                        usable.attributes = new List<Attribute>();
                        foreach (var required in y.RequiredAttributes)
                        {
                            var attr = x.attributes.FirstOrDefault(z=>z.config.@base == required);
                            usable.attributes.Add(attr);
                        }
                        return usable;
                    });
                }

                return new List<Usable>();
            }).ToList();
        }

        public void Update()
        {
            if(movement)
                movement.enabled = health.current > 0f;
            if(contactDamage)
                contactDamage.enabled = health.current > 0f;
            collision.isTrigger = health.current == 0;
        }
    }
}