using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class AttributeDisplay : MonoBehaviour
    {
        public string attributeKey;
        public int value;
        public int allocatedValue;
        public bool allowAllocation;

        [SerializeField] private TextMeshProUGUI attributeText;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Button increaseButton;

        public void Setup(string key, int attrValue, int allocated = 0, bool canIncrease = false,
            bool canDecrease = false,
            Action<int> onValueChangedBy = null)
        {
            attributeKey = key;
            UpdateValue(attrValue, allocated, canIncrease, canDecrease);

            if (onValueChangedBy == null)
            {
                allowAllocation = false;
                decreaseButton.gameObject.SetActive(false);
                increaseButton.gameObject.SetActive(false);
                return;
            }

            allowAllocation = true;
            decreaseButton.onClick.RemoveAllListeners();
            decreaseButton.onClick.AddListener(() => { onValueChangedBy?.Invoke(-1); });

            decreaseButton.interactable = allocatedValue > 0;
            increaseButton.onClick.RemoveAllListeners();
            increaseButton.onClick.AddListener(() => { onValueChangedBy?.Invoke(1); });
        }

        public void UpdateValue(int attrValue, int allocated, bool canIncrease = false, bool canDecrease = false)
        {
            value = attrValue;
            allocatedValue = allocated;
            attributeText.text = attributeKey;
            valueText.text = (value + allocated).ToString();
            increaseButton.interactable = canIncrease;
            decreaseButton.interactable = canDecrease;
        }
    }
}