using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public class ItemBase : StaticData<ItemBase>
    {
        public Sprite icon;
        public List<UsableEffectDefinition> usableDefinitions;
        public List<AttributeConfig> attributesConfigs = new List<AttributeConfig>();

        public Core.Models.Item GetDefaultItemModel()
        {
            var item = new Core.Models.Item
            {
                ItemBaseId = id
            };

            return item;
        }

        private void OnValidate()
        {
            if (usableDefinitions == null) return;
            foreach (var def in usableDefinitions)
            {
                if (!def) continue;
                attributesConfigs ??= new List<AttributeConfig>();
                foreach (var required in def.RequiredAttributes)
                {
                    if (!attributesConfigs.Exists(x => x.@base == required))
                    {
                        attributesConfigs.Add(new AttributeConfig { @base = required });
                    }
                }
            }
        }
    }
}