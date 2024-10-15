using System;
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

        public Transform graphicParent;
        public EquipmentGraphic graphic;

        public void Setup(Data.Equipment newData)
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

            name = data.name;
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