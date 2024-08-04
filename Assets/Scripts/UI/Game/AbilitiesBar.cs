using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class AbilitiesBar : MonoBehaviour
    {
        [SerializeField] private AbilityUIDisplay prefab;
        [SerializeField] private List<AbilityUIDisplay> spawnedAbilities = new();

        [SerializeField] private Transform holder;

        private void LateUpdate()
        {
            if (!PlayerBehaviour.PlayerChar) return;
            if (PlayerBehaviour.PlayerChar.currentAbilities.Count == spawnedAbilities.Count) return;
            var destroy = spawnedAbilities.Where(x => !PlayerBehaviour.PlayerChar.currentAbilities.Contains(x.ability))
                .ToList();
            var add = PlayerBehaviour.PlayerChar.currentAbilities
                .Where(x => !spawnedAbilities.Select(xx => xx.ability).Contains(x)).ToList();
            foreach (var abilityDisplay in destroy)
            {
                Destroy(abilityDisplay.gameObject);
            }

            foreach (var ability in add)
            {
                AddAbility(ability);
            }
        }

        public void AddAbility(Ability ability)
        {
            var abilityDisplay = Instantiate(prefab, holder);
            abilityDisplay.ability = ability;
            abilityDisplay.icon.sprite = ability.uiIcon;
            spawnedAbilities.Add(abilityDisplay);
        }
    }
}