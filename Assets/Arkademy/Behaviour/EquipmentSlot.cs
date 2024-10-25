using Arkademy.Behaviour.Usables;
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
            data.OnEquipmentChanged?.Invoke(data.equipment,newEquipment);
            data.equipment = newEquipment;
            if (template.provideUsable)
            {
                var usable = Instantiate(template.provideUsable, transform) as WeaponSwing;
                Debug.Log(usable);
                if (usable)
                {
                    user.usable = usable;
                    usable.user = user;
                    usable.equipment = equipment;
                }
            }
            return true;
        }
    }
}