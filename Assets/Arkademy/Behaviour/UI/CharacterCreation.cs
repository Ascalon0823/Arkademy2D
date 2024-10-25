using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Templates;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class CharacterCreation : MonoBehaviour
    {
        private List<CharacterTemplate> _characterTemplates;
        [SerializeField] private TMP_InputField characterNameInputField;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button prevTemplate;
        [SerializeField] private Button nextTemplate;
        [SerializeField] private Button cancel;
        [SerializeField] private Button confirm;

        [SerializeField] private int selectedTemplateIdx;
        [SerializeField] private Animator templateDisplayAnimator;
        [SerializeField] private TextMeshProUGUI templateName;

        [SerializeField] private Data.Character currCharacter;
        //
        // [SerializeField] private AttributeDisplay freeAPDisplay;
        // [SerializeField] private AttributeDisplay attributeDisplayPrefab;
        // [SerializeField] private RectTransform attributeDisplayContainer;
        // private List<AttributeDisplay> _createdDisplays = new List<AttributeDisplay>();

        [SerializeField] private CharacterStatus statusPage;
        private void Awake()
        {
            gameObject.SetActive(false);
            characterNameInputField.onValueChanged.AddListener(s =>
            {
                currCharacter.name = s;
                confirm.interactable = !string.IsNullOrEmpty(s);
            });
            prevTemplate.onClick.RemoveAllListeners();
            prevTemplate.onClick.AddListener(() => { MoveIdxBy(-1); });
            nextTemplate.onClick.RemoveAllListeners();
            nextTemplate.onClick.AddListener(() => MoveIdxBy(1));
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        public void Activate(Action<Data.Character> onConfirm)
        {
            _characterTemplates = Resources.LoadAll<CharacterTemplate>("").Where(x=>x.useForCharaCreation).ToList();
            prevTemplate.interactable = _characterTemplates.Count > 1;
            nextTemplate.interactable = _characterTemplates.Count > 1;
            UpdateTemplateDisplay();
            confirm.interactable = false;
            confirm.onClick.RemoveAllListeners();
            confirm.onClick.AddListener(() =>
            {
                onConfirm?.Invoke(currCharacter);
                gameObject.SetActive(false);
            });
            gameObject.SetActive(true);
        }

        private void MoveIdxBy(int value)
        {
            selectedTemplateIdx += value;
            selectedTemplateIdx = selectedTemplateIdx < 0
                ? _characterTemplates.Count - 1
                : selectedTemplateIdx % _characterTemplates.Count;
            UpdateTemplateDisplay();
        }

        private void UpdateTemplateDisplay()
        {
            var template = _characterTemplates[selectedTemplateIdx];
            templateDisplayAnimator.runtimeAnimatorController = template.animationController;
            templateName.text = template.name;
            currCharacter = template.GetNewCharacter();
            statusPage.Setup(currCharacter);
        }
    }
}