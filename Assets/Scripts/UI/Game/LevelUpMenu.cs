using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.UI.Game
{
    public class LevelUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        public CharacterBehaviour character;

        [SerializeField] private Transform contents;
        private List<LevelUpOptionItem> _spawnedItems = new();
        [SerializeField] private LevelUpOptionItem itemPrefab;

        public void Start()
        {
            PlayerBehaviour.Player.onPlayerCharLevelUp.AddListener(chara =>
            {
                character = chara;
                Toggle(true);
            });
            menuPanel.SetActive(false);
        }

        public void Toggle(bool active)
        {
            if (active != menuPanel.activeSelf)
            {
                PlayerBehaviour.Player.pauseCount += active ? 1 : -1;
            }

            menuPanel.SetActive(active);
            if (!active) return;
            foreach (var item in _spawnedItems)
            {
                Destroy(item.gameObject);
            }

            _spawnedItems.Clear();
            var abilityData = Database.GetDatabase().abilityData.ToList();
            for (var i = 0; i < Mathf.Min(abilityData.Count, 3); i++)
            {
                var idx = Random.Range(0, abilityData.Count);
                var data = abilityData[idx];
                abilityData.RemoveAt(idx);
                var option = Instantiate(itemPrefab, contents);
                option.abilityName.text = data.name;
                option.data = data;
                option.menu = this;
                _spawnedItems.Add(option);
            }
        }

        public void Toggle()
        {
            Toggle(!menuPanel.activeSelf);
        }

        public void SelectLevelUp(Database.AbilityData abilityData)
        {
            LevelUp(abilityData);
            Toggle(false);
            character.ResolveLevelUp();
        }

        public void LevelUp(Database.AbilityData abilityData)
        {
            var charaAbi = character.currentAbilities.FirstOrDefault(x => x.abilityName == abilityData.name);
            if (charaAbi)
            {
                charaAbi.level += 1;
                return;
            }
            character.AddAbility(abilityData);
        }
    }
}