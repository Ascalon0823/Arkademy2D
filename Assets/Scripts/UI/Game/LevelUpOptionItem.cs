using TMPro;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class LevelUpOptionItem : MonoBehaviour
    {
        public Database.AbilityData data;
        public LevelUpMenu menu;
        public TextMeshProUGUI abilityName;
        public void OnClick()
        {
            menu.SelectLevelUp(data);
        }
    }
}