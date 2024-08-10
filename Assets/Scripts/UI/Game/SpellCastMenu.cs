using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arkademy.UI.Game
{
    public class SpellCastMenu : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject button;
        public string spellKey;
        public Spell currentSpell;

        public void BeginSpellUse(Vector2 delta)
        {
            var spellData = Database.GetDatabase().spellData.FirstOrDefault(x => x.spellKey == spellKey);
            if (spellData == null)
            {
                ToggleMenu(false);
                return;
            }

            currentSpell = Instantiate(spellData.prefab, PlayerBehaviour.PlayerChar.transform.position,
                Quaternion.identity);
            currentSpell.gameObject.SetLayerRecursive(PlayerBehaviour.PlayerChar.gameObject.layer);
            currentSpell.user = PlayerBehaviour.PlayerChar;
            var canUse = currentSpell.OnUse(new Spell.SpellUsage { Phase = Spell.Phase.Begin, Direction = delta });
            if (!canUse) ToggleMenu(false);
        }

        public void UpdateSpellUse(Vector2 delta)
        {
            if (!currentSpell || !currentSpell.OnUse(new Spell.SpellUsage
                    { Phase = Spell.Phase.Update, Direction = delta }))
            {
                ToggleMenu(false);
            }
        }

        public void EndSpellUse(Vector2 delta)
        {
            if (currentSpell)
            {
                currentSpell.OnUse(new Spell.SpellUsage
                    { Phase = Spell.Phase.End, Direction = delta });
            }
            ToggleMenu(false);
        }

        public void ToggleMenu(bool active)
        {
            if (active)
            {
                if (!PlayerBehaviour.PlayerChar) return;
                
            }
            PlayerBehaviour.PlayerChar.canLevelUp = !active;
            if (!active)
            {
                PlayerBehaviour.PlayerChar.ResolveLevelUp();
            }
            panel.SetActive(active);
            button.SetActive(active);
            
        }
    }
}