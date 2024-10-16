using Arkademy.Behaviour;
using UnityEngine;
using Equipment = Arkademy.Data.Equipment;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Equipment Template", menuName = "Template/Equipment", order = 0)]
    public class EquipmentTemplate : ScriptableObject
    {
        public Equipment templateData;
        public Behaviour.Equipment equippedPrefab;
        [Header("Graphics")] public Sprite equipmentSprite;
        public RuntimeAnimatorController equipmentAnimation;
        public Behaviour.EquipmentGraphic equippedGraphicPrefab;
        public Sprite equipmentUISprite;
        public Usable provideUsable;
        
        
        private void OnEnable()
        {
            templateData.templateName = name;
        }
        public Equipment GetNewEquipment()
        {
            return new Equipment(templateData);
        }
    }
}