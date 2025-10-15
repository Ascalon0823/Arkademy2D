using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Midterm.Character
{
    public class AbilityHUD : MonoBehaviour
    {
        public RectTransform holder;

        public AbilityHUDItem itemPrefab;
        public List<AbilityHUDItem> items = new();

        private void LateUpdate()
        {
            var charaAbilities = Player.Player.Local.currCharacter.abilities;
            var toRemove = items.Where(x => !charaAbilities.Contains(x.targetAbility)).ToList();
            foreach (var remove in toRemove)
            {
                items.Remove(remove);
                Destroy(remove.gameObject);
            }
            var toAdd = charaAbilities.Where(x => items.All(z => z.targetAbility != x)).ToList();
            foreach (var addAbility in toAdd)
            {
                var item = Instantiate(itemPrefab, holder);
                item.targetAbility = addAbility;
                items.Add(item);
            }
        }
    }
}