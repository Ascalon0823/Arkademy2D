using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Arkademy.UI.Game
{
    public class LevelUpOptionItem : MonoBehaviour
    {
        public Database.AbilityData data;
        public LevelUpMenu menu;
        public TextMeshProUGUI abilityName;
        public Image icon;
        public void OnClick()
        {
            menu.SelectLevelUp(data);
        }
    }
}