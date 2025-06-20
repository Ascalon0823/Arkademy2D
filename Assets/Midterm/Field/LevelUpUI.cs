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


        private void Start()
        {
            Toggle(false);
        }

        public void Toggle(bool active)
        {
            if (active)
            {
                if (levelUpEvent.Count == 0) return;
                PopulateList();
            }

            Time.timeScale = active?0:1;
            gameObject.SetActive(active);
        }


        public LevelUpUIItem selectedAbilityItem;

        public void Select(LevelUpUIItem item)
        {
            selectedAbilityItem = item;
        }

        public void Confirm()
        {
            Player.Player.Local.currCharacter.LevelUpAbility(selectedAbilityItem.holdAbility);
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
            var availableAbility = WaveManager.Instance.availableAbilities.ToList();
            var repeat = Mathf.Min(3, availableAbility.Count);
            selectedAbilityItem = null;
            for (var i = 0; i < repeat; i++)
            {
                var idx = Random.Range(0, availableAbility.Count);
                var ability = availableAbility[idx];
                availableAbility.RemoveAt(idx);
                var item = Instantiate(levelUpItemPrefab, container);
                item.SetupAs(ability, this);
                spawnedItems.Add(item);
                if (!selectedAbilityItem) Select(item);
            }
        }
    }
}