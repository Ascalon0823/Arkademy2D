using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class EquipmentChanger : Interactable
    {
        public Equipment equipment;

        public override bool OnInteractedBy(Character character)
        {
            var baseData = ItemBase.GetItemBase(equipment.baseName);
            if (baseData == null || !baseData.isEquipment) return false;
            var slot = character.data.equipmentSlots.FirstOrDefault(x=>x.slot == baseData.slot);
            if(slot == null) return false;
            var curr = slot.equipment;
            character.ChangeEquipment(equipment);
            equipment = curr;
            return true;
        }
    }
}