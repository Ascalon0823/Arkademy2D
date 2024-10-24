using System;
using System.Linq;
using Arkademy.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class FieldDisplay : MonoBehaviour
    {
        public TextMeshProUGUI keyText;
        public TextMeshProUGUI valueText;
        public Button increaseButton;
        public Button decreaseButton;

        private ISubscription handle;
        private Field field;

        public void SetButtonInteractable(bool increaseButtonInteractable, bool decreaseButtonInteractable)
        {
            increaseButton.interactable = increaseButtonInteractable;
            decreaseButton.interactable = decreaseButtonInteractable;
        }

        public void Bind(Field newField, bool allowDecrease = false,
            bool allowIncrease = false,
            Action<int> onValueChanged = null,
            Func<Field, string> toString = null, string newKetText = null)
        {
            handle?.Dispose();
            field = newField;
            var binding =new Action<long>((curr) =>
            {
                Setup(newKetText??field.key, toString == null ? field.GetValue().ToString() : toString.Invoke(field),
                    allowDecrease, allowIncrease, (c) =>
                    {
                        onValueChanged?.Invoke(c);
                        valueText.text = toString == null ? field.GetValue().ToString() : toString.Invoke(field);
                    });
            });

            handle = field.Subscribe(binding);
        }

        public void Setup(string key, string value, bool allowDecrease, bool allowIncrease, Action<int> onValueChanged)
        {
            keyText.text = key;
            valueText.text = value;
            decreaseButton.gameObject.SetActive(allowDecrease);
            increaseButton.gameObject.SetActive(allowIncrease);
            increaseButton.onClick.RemoveAllListeners();
            increaseButton.onClick.AddListener(() => onValueChanged(1));
            decreaseButton.onClick.RemoveAllListeners();
            decreaseButton.onClick.AddListener(() => onValueChanged(-1));
        }

        private void OnDestroy()
        {
            handle?.Dispose();
        }
    }
}