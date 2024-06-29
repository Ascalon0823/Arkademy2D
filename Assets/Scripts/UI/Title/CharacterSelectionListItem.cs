using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI.Title
{
    public class CharacterSelectionListItem : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public Image characterIcon;
        public Animator iconAnimator;
        public Button characterSelectButton;

        public void Select(bool selected)
        {
            iconAnimator.Update(0);
            iconAnimator.enabled = selected;
        }
    }
}