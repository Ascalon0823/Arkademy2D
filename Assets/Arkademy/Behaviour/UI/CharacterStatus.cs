using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class CharacterStatus : NamedPage
    {
        public Data.Character character;
        public FieldDisplay charaName;
        public FieldDisplay level;
        public FieldDisplay xP;
        public FieldDisplay aP;
        public bool allowDecreasePoints;

        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private FieldDisplay fieldDisplayPrefab;
        [SerializeField] private List<FieldDisplay> fieldDisplayList = new();
        private List<Data.ISubscription> handles = new List<Data.ISubscription>();

        public override void OnActivated(bool activated)
        {
            base.OnActivated(activated);
            if (Game.localPlayers != null && Game.localPlayers.Count > 0 && Game.localPlayers[0].controllingCharacter)
                Setup(Game.localPlayers[0].controllingCharacter.data);
        }

        private void Update()
        {
            // if (Application.isEditor)
            // {
            //     foreach (var handle in handles)
            //     {
            //         handle.Trigger();
            //     }
            // }
        }

        public void Setup(Data.Character newCharacter)
        {
            if (character == newCharacter) return;
            character = newCharacter;
            charaName.valueText.text = character.name;
            if (character.TryGetAttr(Data.Character.Lv, out var lv))
            {
                level.Bind(lv);
            }

            if (character.TryGetAttr(Data.Character.XP, out var xp))
            {
                xP.Bind(xp);
            }

            if (character.TryGetAttr(Data.Character.AP, out var ap))
            {
                aP.Bind(ap);
            }
            else
            {
                Debug.Log($"No progression found for character {Data.Character.AP}");
            }

            foreach (var fd in fieldDisplayList)
            {
                Destroy(fd.gameObject);
            }

            foreach (var handle in handles)
            {
                handle?.Dispose();
            }

            handles.Clear();
            fieldDisplayList.Clear();
            foreach (var allocatable in character.attributes.Where(x =>
                         x.persistentModifiers.Any(xx => xx.key == "AP Allocation")))
            {
                var fd = Instantiate(fieldDisplayPrefab, contentRoot);
                fieldDisplayList.Add(fd);
                var growthIdx = allocatable.persistentModifiers.FindIndex(x => x.key == "AP Allocation");
                var growth = allocatable.persistentModifiers[growthIdx];
                fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0 && allowDecreasePoints);
                fd.Bind(allocatable, allowDecreasePoints, true, (diff) =>
                    {
                        growth.Value += diff;
                        ap.Value -= diff;
                        fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0 && allowDecreasePoints);
                    }, f =>
                    {
                        var v = allocatable.GetValue();
                        return $"{v}";
                    },
                    Data.Character.GetAbbreviation(allocatable.key));
                handles.Add(ap.Subscribe((curr) =>
                {
                    fd.SetButtonInteractable(curr > 0, growth.Value > 0 && allowDecreasePoints);
                }));
            }
        }
    }
}