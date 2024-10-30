using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class Equipments : NamedPage
    {
        public List<EquipmentHolder> holders = new List<EquipmentHolder>();
        public Data.Character character;

        public override void OnActivated(bool activated)
        {
            base.OnActivated(activated);
            if (Game.localPlayers != null && Game.localPlayers.Count > 0 && Game.localPlayers[0].controllingCharacter)
                Setup(Game.localPlayers[0].controllingCharacter.data);
        }

        public void Setup(Data.Character newCharacter)
        {
            if (character == newCharacter) return;
            character = newCharacter;
            foreach (var holder in holders)
            {
                var slot = character.slots.Find(x => x.category == holder.category && holder.slot != x);
                if (slot == null)
                {
                    holder.gameObject.SetActive(false);
                    continue;
                }
                holder.SetupSlot(slot);
            }
        }
    }
}