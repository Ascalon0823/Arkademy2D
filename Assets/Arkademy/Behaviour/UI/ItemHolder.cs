using Arkademy.Configs;
using Arkademy.Data;
using Arkademy.Templates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class ItemHolder : MonoBehaviour
    {
        public Item item;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private Image rarityColor;
        [SerializeField] private Image spriteHolder;

        public virtual void Setup<T>(T newItem) where T : Item
        {
            item = newItem;
            if (item == null || string.IsNullOrEmpty(item.templateName))
            {
                rarityColor.color =RarityConfig.Instance.TryGetConfig(0, out var defaultConfig)
                    ? defaultConfig.color
                    : rarityColor.color;
                spriteHolder.sprite = null;
                spriteHolder.enabled = false;
                return;
            }
            rarityColor.color = RarityConfig.Instance.TryGetConfig(item.rarity, out var config)
                ? config.color
                : rarityColor.color;
            var template = ItemTemplate<T>.GetTemplate(item);
            spriteHolder.sprite = template.uiSprite;
            spriteHolder.enabled = true;
        }
    }
}