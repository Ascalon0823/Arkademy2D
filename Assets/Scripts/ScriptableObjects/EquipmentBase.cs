using System;
using UnityEngine;

namespace Arkademy
{
   

    [CreateAssetMenu(fileName = "New Equipment Base", menuName = "Data/Add Equipment Base", order = 0)]
    public class EquipmentBase : ScriptableObject
    {
        public Equipment baseData;
    }
    
    [Serializable]
    public class EquipmentSlot
    {
        public enum Category
        {
            MainHand,
            OffHand,
            Head,
            Shoulder,
            Body,
            Hand,
            Feet,
            Face,
            Accessory
        }

        public Category category;
        public Equipment equipment;
    }

    [Serializable]
    public class Equipment
    {
        public enum Category
        {
            OneHandWeapon,
            TwoHandWeapon,
            OffHandWeapon,
            Shield,
            Helmet,
            Cloak,
            BodyArmor,
            Glove,
            Shoe,
            Mask,
            Accessory
        }

        public enum Rarity
        {
            Normal,
            Magical,
            Rare,
            Mythical,
            Unique
        }

        public string name;
        public Category category;
        public Rarity rarity;
        public int capacity;
        public Affix[] affixes;
    }

    [Serializable]
    public class Affix
    {
        public int cost;
    }
}