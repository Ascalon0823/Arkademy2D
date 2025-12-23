using System.Linq;
using Arkademy2D.Core.Models;
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
            if (item.Model.Extras.TryGetExtra(typeof(Weapon).Name,out Weapon weapon))
            {
                context.meleeContext = new MeleeContext
                {
                    Power = weapon.damage,
                    Range = (item.itemBase.ItemExtraProviders.Where(x=>x is WeaponBase)
                        .FirstOrDefault() as WeaponBase).range
                };
            }
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