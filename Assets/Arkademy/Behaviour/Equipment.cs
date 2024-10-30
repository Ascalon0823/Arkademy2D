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
        public bool rotateToFace;
        public bool flipToFace;

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
                    if (usable is WeaponSwing ws)
                        ws.equipment = this;
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

        private void Update()
        {
            if (rotateToFace)
            {
                graphicParent.up = facingDir;
            }
            else if (flipToFace)
            {
                graphicParent.up = Vector2.Dot(facingDir, Vector2.left) >= 0 ? Vector2.left : Vector2.right;
            }
            else
            {
                return;
            }

            graphicParent.localScale = new Vector3(Vector2.Dot(facingDir, Vector2.left) >= 0 ? 1 : -1, 1, 1);
        }
    }
}