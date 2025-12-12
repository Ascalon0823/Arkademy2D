using System;
using Arkademy2D.Common;
using Arkademy2D.Title.Behaviour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Title.Scripts.UI
{
    public class CharacterSelectionItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Button button;
        public CharacterData characterData { get; private set; }
        private Action<CharacterSelectionItem> _onSelected;

        public void Setup(CharacterData data, Action<CharacterSelectionItem> onSelected)
        {
            characterData = data;
            characterNameText.text = data.DisplayName;
            _onSelected = onSelected;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
            SetSelected(false);
        }

        public void OnClick()
        {
            _onSelected?.Invoke(this);
        }

        public void SetSelected(bool isSelected)
        {
            characterNameText.color = isSelected ? Color.white : Color.gray;
        }
    }
}