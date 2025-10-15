using Midterm.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Midterm.Field
{
    public class LevelUpUIItem : MonoBehaviour
    {
        public LevelUpUI parent;
        public Image icon;
        public TextMeshProUGUI abilityNameText;

        public Ability holdAbility;
        public Ability.Upgrade holdUpgrade;
        public void SetupAs(Ability ability,Ability.Upgrade upgrade, LevelUpUI list)
        {
            icon.sprite = ability.icon;
            abilityNameText.text = ability.internalName + (upgrade == null ? "" : $" {upgrade.name}+1 = {upgrade.currLevel+1}");
            parent = list;
            holdAbility = ability;
            holdUpgrade = upgrade;
        }

        public void Select()
        {
            parent.Select(this);
        }
    }
}