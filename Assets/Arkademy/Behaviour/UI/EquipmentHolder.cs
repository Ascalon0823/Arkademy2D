using System;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class EquipmentHolder : ItemHolder
    {
        public Data.EquipmentSlot slot;
        public Data.EquipmentSlot.Category category;
        public void SetupSlot(Data.EquipmentSlot newSlot)
        {
            slot = newSlot;
            if (!string.IsNullOrEmpty(slot.equipment.templateName))
            {
                Setup(slot.equipment);
            }
            slot.OnEquipmentChanged += SetupEquipment;
        }

        private void OnDestroy()
        {
            slot.OnEquipmentChanged -= SetupEquipment;
        }

        private void SetupEquipment(Data.Equipment oldEquipment, Data.Equipment equipment)
        {
            Setup(equipment);
        }
    }
}