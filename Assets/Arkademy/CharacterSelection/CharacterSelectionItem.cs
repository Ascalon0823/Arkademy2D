using Arkademy.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Arkademy.CharacterSelection
{
    public class CharacterSelectionItem : MonoBehaviour
    {
        public CharacterSelectionMenu menu;
        public CharacterRecord record;
        public bool setupDone;
        public bool selected;
        public GameObject selectionHighlight;
        public GameObject addSign;
        public GameObject characterDisplay;
        public TextMeshProUGUI characterDisplayText;
        public Animator characterDisplayAnimator;
        private static readonly int Walking = Animator.StringToHash("walking");

        private void Awake()
        {
            selectionHighlight.gameObject.SetActive(false);
            characterDisplay.SetActive(false);
        }

        public void Setup(CharacterRecord setupRecord)
        {
            record = setupRecord;
            setupDone = true;
            addSign.gameObject.SetActive(false);
            characterDisplay.SetActive(true);
            characterDisplayText.text = record.character.displayName;
            // var template = Resources.Load<CharacterTemplate>(setupRecord.characterData.templateName);
            // characterDisplayAnimator.runtimeAnimatorController = template.animationController;
        }

        public void Select(bool isSelected)
        {
            if (!setupDone)
            {
                return;
            }

            selected = isSelected;
            selectionHighlight.gameObject.SetActive(isSelected);
            characterDisplayAnimator.SetBool(Walking, isSelected);
        }

        public void OnClick()
        {
            if (!setupDone)
            {
                SceneManager.LoadScene("CharacterCreation");
                return;
            }

            menu.SelectItem(this);
        }
    }
}