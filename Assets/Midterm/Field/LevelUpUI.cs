using System;
using System.Collections.Generic;
using System.Linq;
using Midterm.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Field
{
    public class LevelUpUI : MonoBehaviour
    {
        public Queue<int> levelUpEvent = new Queue<int>();


        public RectTransform container;
        public LevelUpUIItem levelUpItemPrefab;
        [SerializeField] private List<LevelUpUIItem> spawnedItems = new List<LevelUpUIItem>();

        public AudioSource audioSource;

        private void Start()
        {
            Toggle(false);
        }

        public void Toggle(bool active)
        {
           

            Time.timeScale = active?0:1;
            gameObject.SetActive(active);
            if (active)
            {
                if (levelUpEvent.Count == 0) return;
                PopulateList();
                if(audioSource)
                    audioSource.Play();
            }
        }


        public LevelUpUIItem selectedAbilityItem;

        public void Select(LevelUpUIItem item)
        {
            selectedAbilityItem = item;
        }

        public void Confirm()
        {
            Player.Player.Local.currCharacter.LevelUpAbility(selectedAbilityItem.holdAbility,selectedAbilityItem.holdUpgrade);
            Toggle(false);
            if (levelUpEvent.Count == 0) return;
            Toggle(true);
        }

        public void PopulateList()
        {
            foreach (var item in spawnedItems)
            {
                Destroy(item.gameObject);
            }

            spawnedItems.Clear();
            var lastEvent = levelUpEvent.Dequeue();
            //var availableAbility = WaveManager.Instance.availableAbilities.ToList();
            var availableOption = Player.Player.Local.currCharacter.abilities
                .SelectMany(x => x.GetAvailableUpgrades().Select(z => (x, z)))
                .ToList();
            var newAbilities = WaveManager.Instance.availableAbilities.Where(x =>
                Player.Player.Local.currCharacter.abilities.All(z => z.internalName != x.internalName)).ToList();
            foreach (var newAbility in newAbilities)
            {
                availableOption.Add((newAbility,null));
            }
            var repeat = Mathf.Min(3, availableOption.Count);
            selectedAbilityItem = null;
            for (var i = 0; i < repeat; i++)
            {
                var idx = Random.Range(0, availableOption.Count);
                var abilityOption = availableOption[idx];
                availableOption.RemoveAt(idx);
                var item = Instantiate(levelUpItemPrefab, container);
                item.SetupAs(abilityOption.x, abilityOption.z,this);
                spawnedItems.Add(item);
                if (!selectedAbilityItem) Select(item);
            }
        }
    }
}