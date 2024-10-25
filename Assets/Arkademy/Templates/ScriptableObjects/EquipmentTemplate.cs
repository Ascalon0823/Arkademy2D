using Arkademy.Behaviour;
using UnityEngine;
using Equipment = Arkademy.Data.Equipment;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Equipment Template", menuName = "Template/Equipment", order = 0)]
    public class EquipmentTemplate : ItemTemplate<Equipment>
    {
        public Behaviour.Equipment equippedPrefab;
        [Header("Graphics")] public Sprite equipmentSprite;
        public RuntimeAnimatorController equipmentAnimation;
        public Behaviour.EquipmentGraphic equippedGraphicPrefab;
        public Usable provideUsable;

        protected override void OnEnable()
        {
            templateData.templateName = name;
        }

        public Equipment GetNewEquipment()
        {
            return templateData.Copy();
        }
    }
}