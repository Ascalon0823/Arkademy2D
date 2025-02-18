using UnityEngine;

namespace Arkademy.Data.Scriptable
{
    [CreateAssetMenu(fileName = "ItemBase", menuName = "Data/ItemBase", order = 0)]
    public class ItemBaseObject : ScriptableObject
    {
        public ItemBase itemBase;
    }
}