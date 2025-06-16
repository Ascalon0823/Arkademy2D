using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Arkademy.Gameplay.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    public class AbilityPage : Page
    {
        public AbilitySlotItem[] items;
        public override void OnShow()
        {
            base.OnShow();
            ToggleSelection(false);
            foreach (var item in items)
            {
                item.Init();
            }
        }

        [SerializeField] private CanvasGroup selectionPage;
        [SerializeField] private AbilitySlotItem currSelectionSlot;
        [SerializeField] private string selectedAbilityName;
        [SerializeField] private Transform content;
        [SerializeField] private AbilitySelectionItem itemPrefab;
        private List<AbilitySelectionItem> _selectionItems = new();

        [SerializeField] private Image selectionImage;
        [SerializeField] private TextMeshProUGUI selectionText;
        public void UpdateSelection(string selected, string displayName, Sprite icon)
        {
            selectionImage.sprite = icon;
            selectionImage.enabled = icon;
            selectedAbilityName = selected;
            selectionText.text = string.IsNullOrEmpty(selected) ? "None" : displayName;
        }
        public void BeginSelection(AbilitySlotItem item)
        {
            UpdateSelection(item.abilityName,item.abilityText.text, item.icon.sprite);
            currSelectionSlot = item;
            selectedAbilityName = item.abilityName;
            foreach (var selectionItem in _selectionItems)
            {
                Destroy(selectionItem.gameObject);
            }
            _selectionItems.Clear();
            var characterRecord = Session.currCharacterRecord;
            var abilities = characterRecord.character.GetAvailableAbilities().Select(x => Ability.GetAbility(x))
                .Where(x => x.abilitySlotType == item.slotType).ToArray();
            foreach (var ability in abilities)
            {
                var selectionItem = Instantiate(itemPrefab, content);
                _selectionItems.Add(selectionItem);
                selectionItem.icon.sprite = ability.icon;
                selectionItem.text.text = ability.displayName;
                selectionItem.button.onClick.AddListener(() =>
                {
                    UpdateSelection(ability.name,ability.displayName, ability.icon);
                });
                selectionItem.empty.SetActive(false);
            }
            var emptyItem =Instantiate(itemPrefab, content);
            _selectionItems.Add(emptyItem);
            emptyItem.empty.SetActive(true);
            emptyItem.button.onClick.AddListener(() =>
            {
                UpdateSelection(string.Empty, "None", null);
            });
            ToggleSelection(true);
        }

        private void ToggleSelection(bool on)
        {
            selectionPage.alpha = on ? 1 : 0;
            selectionPage.interactable = on;
            selectionPage.blocksRaycasts = on;
        }
        public void CancelSelection()
        {
            ToggleSelection(false);
        }

        public void ConfirmSelection()
        {
            ToggleSelection(false);
            currSelectionSlot.SetAbility(selectedAbilityName);
        }
    }
}