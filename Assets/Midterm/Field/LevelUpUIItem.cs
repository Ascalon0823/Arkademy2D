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
        public void SetupAs(Ability ability, LevelUpUI list)
        {
            icon.sprite = ability.icon;
            abilityNameText.text = ability.name;
            parent = list;
            holdAbility = ability;
        }

        public void Select()
        {
            parent.Select(this);
        }
    }
}