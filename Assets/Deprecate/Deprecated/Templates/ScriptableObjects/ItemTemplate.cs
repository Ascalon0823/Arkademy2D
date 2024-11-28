using System;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Item Template", menuName = "Template/Item", order = 0)]
    public class ItemTemplate : ItemTemplate<Item>
    {
        
    }
    public abstract class ItemTemplate<T> : ScriptableObject where T : Item
    {
        public T templateData;
        public Sprite uiSprite;

        protected virtual void OnEnable()
        {
            templateData.templateName = name;
        }

        public static ItemTemplate<T> GetTemplate(Item item)
        {
            return Resources.Load<ItemTemplate<T>>(item.templateName);
        }
    }
}