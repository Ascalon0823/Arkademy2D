using Midterm.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Midterm.Player
{
    public class PlayableCharacterSelectionItem : MonoBehaviour
    {
        public PlayableCharacterData data;
        public Image icon;
        public PlayableCharacterSelection parent;
        public TextMeshProUGUI costText;

        public void SetupAs(PlayableCharacterSelection list, PlayableCharacterData playerCharacterData, bool isUnlocked)
        {
            parent = list;
            data = playerCharacterData;
            icon.color = isUnlocked ? Color.white : Color.grey;
            icon.sprite = data.icon;
            costText.text = isUnlocked ? "" : $"${data.unlockPrice}";
        }

        public void OnSelected()
        {
            parent.Select(this);
        }
    }
}