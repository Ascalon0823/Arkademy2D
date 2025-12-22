using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class User : MonoBehaviour
    {
        public void UseItem(ItemData item, UseContext context)
        {
            foreach (var usableData in item.usableData)
            {
                if (usableData.Use(context))
                {
                    Debug.Log($"{name} uses {item.itemBase.displayName}");
                }
            }
        }
    }
}