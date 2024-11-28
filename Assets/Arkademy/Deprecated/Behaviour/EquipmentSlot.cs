using Arkademy.Behaviour.Usables;
using Arkademy.Data;
using Arkademy.Templates;
using Newtonsoft.Json;
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
            if (equipment)
            {
                equipment.UnEquipFrom();
                Destroy(equipment.gameObject);
            }

            if (newEquipment == null || !newEquipment.Valid()) return true;
            Debug.Log("change equip");
            var template = Resources.Load<EquipmentTemplate>(newEquipment.templateName);
            equipment = Instantiate(template.equippedPrefab, transform);
            equipment.Setup(newEquipment, user);
            data.OnEquipmentChanged?.Invoke(data.equipment, newEquipment);
            data.equipment = newEquipment;
            return true;
        }
    }
}