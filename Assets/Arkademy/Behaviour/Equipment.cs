using System;
using Arkademy.Behaviour.Usables;
using Arkademy.Templates;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Equipment : MonoBehaviour
    {
        public Data.Equipment data;
        public Vector2 facingDir;
       

        [SerializeField] private Usable providedUsable;
        public Transform graphicParent;
        public EquipmentGraphic graphic;
        
        public void Setup(Data.Equipment newData, Character user)
        {
            if (string.IsNullOrEmpty(newData.templateName)) return;
            var template = Resources.Load<EquipmentTemplate>(newData.templateName);
            if (!template) return;
            data = newData;
            if (template.equippedGraphicPrefab)
            {
                graphic = Instantiate(template.equippedGraphicPrefab, graphicParent);
                graphic.spriteRenderer.sprite = template.equipmentSprite;
                graphic.animator.runtimeAnimatorController = template.equipmentAnimation;
                graphic.equipment = this;
            }
            if (data.affixesWhenEquip != null)
            {
                foreach (var affix in data.affixesWhenEquip)
                {
                    affix.OnEquippedTo(user.data, newData);
                }
            }
            if (template.provideUsable)
            {
                var usable = Instantiate(template.provideUsable, transform);
                if (usable)
                {
                    user.usable = usable;
                    usable.user = user;
                    if (usable is EquipmentAbility ea)
                        ea.equipment = this;
                    providedUsable = usable;
                }
            }

            name = data.name;
        }

        public void UnEquipFrom()
        {
            if (data.affixesWhenEquip != null)
            {
                foreach (var affix in data.affixesWhenEquip)
                {
                    affix.OnRemoved();
                }
            }
        }

      
    }
}