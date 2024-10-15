using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Configs.ScriptableObjects;
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

        [SerializeField] private AttributeDisplay freeAPDisplay;
        [SerializeField] private AttributeDisplay attributeDisplayPrefab;
        [SerializeField] private RectTransform attributeDisplayContainer;
        private List<AttributeDisplay> _createdDisplays = new List<AttributeDisplay>();

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
            _characterTemplates = Resources.LoadAll<CharacterTemplate>("").ToList();
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
            SetupAttributeAllocation();
        }

        private void SetupAttributeAllocation()
        {
            foreach (var ad in _createdDisplays)
            {
                Destroy(ad.gameObject);
            }

            _createdDisplays.Clear();
            var config = Resources.Load<AttributePointsAllocationConfig>("AP Allocation Config");
            if (!config) return;
            if (!currCharacter.TryGetAttribute(config.freeAPKey, out var ap, out _)) return;
            freeAPDisplay.Setup(config.freeAPKey, ap.value, 0);
            if (config.allocatableAttributes == null) return;
            foreach (var key in config.allocatableAttributes)
            {
                if (!currCharacter.TryGetAttribute(key, out var attr, out var allocation)) continue;
                var ad = Instantiate(attributeDisplayPrefab, attributeDisplayContainer);
                ad.Setup(attr.key, attr.value, allocation.value, ap.value > 0, allocation.value > 0, diff =>
                {
                    if (!currCharacter.TryGetAttribute(key, out attr, out allocation)) return;
                    if (!currCharacter.TryGetAttribute(config.freeAPKey, out ap, out _)) return;
                    if (ap.value <= 0 && diff > 0) return;
                    ap.value -= diff;
                    allocation.value += diff;
                    if (!currCharacter.TryUpdateAttribute(ap.key, ap.value)) return;
                    if (!currCharacter.TryUpdateAllocation(attr.key, allocation.value)) return;
                    freeAPDisplay.UpdateValue(ap.value, 0);
                    ad.UpdateValue(attr.value, allocation.value, ap.value > 0, allocation.value > 0);
                    foreach (var attrDisplay in _createdDisplays)
                    {
                        if (!attrDisplay.allowAllocation) continue;
                        attrDisplay.UpdateValue(attrDisplay.value, attrDisplay.allocatedValue,
                            ap.value > 0, attrDisplay.allocatedValue > 0
                        );
                    }
                });
                _createdDisplays.Add(ad);
            }

            foreach (var attr in currCharacter.attributes)
            {
                if (config.freeAPKey == attr.key || config.allocatableAttributes.Contains(attr.key)) continue;
                var ad = Instantiate(attributeDisplayPrefab, attributeDisplayContainer);
                ad.Setup(attr.key, attr.value);
                _createdDisplays.Add(ad);
            }
        }
    }
}