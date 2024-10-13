using System;
using Arkademy.Templates.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class CharacterListItem : MonoBehaviour
    {
        public CharacterList list;
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
            characterDisplayText.text = record.characterData.name;
            var template = Resources.Load<CharacterTemplate>(setupRecord.characterData.templateName);
            characterDisplayAnimator.runtimeAnimatorController = template.animationController;
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
                list.BeginCharacterCreation();
                return;
            }

            list.SelectItem(this);
        }
    }
}