using Arkademy.Data;
using Arkademy.Templates;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class EquipmentSlot : MonoBehaviour
    {
        public Data.EquipmentSlot data;
        public Character user;
        public Equipment equipment;

        public void Setup(Data.EquipmentSlot newData, Character newUser)
        {
            data = newData;
            user = newUser;
            if (data.equipment != null && !string.IsNullOrEmpty(data.equipment.templateName))
            {
                Equip(data.equipment);
            }
        }

        public bool Equip(Data.Equipment newEquipment)
        {
            if (equipment) return false;
            var template = Resources.Load<EquipmentTemplate>(newEquipment.templateName);
            equipment = Instantiate(template.equippedPrefab, transform);
            equipment.Setup(newEquipment);
            data.equipment = newEquipment;
            return true;
        }
    }
}