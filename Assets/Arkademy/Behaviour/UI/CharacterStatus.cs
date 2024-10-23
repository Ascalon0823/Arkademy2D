using System.Collections.Generic;
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
        private List<ReactiveFields.Field.Handle> handles = new List<ReactiveFields.Field.Handle>();

        public override void OnActivated(bool activated)
        {
            base.OnActivated(activated);
            if (Game.localPlayers != null && Game.localPlayers.Count > 0 && Game.localPlayers[0].controllingCharacter)
                Setup(Game.localPlayers[0].controllingCharacter.data);
        }

        public void Setup(Data.Character newCharacter)
        {
            if (character == newCharacter) return;
            character = newCharacter;
            charaName.valueText.text = character.name;
            if (character.progression.TryGet(Data.Character.Lv, out var lv))
            {
                level.Bind(lv);
            }

            if (character.progression.TryGet(Data.Character.XP, out var xp))
            {
                xP.Bind(xp);
            }

            if (character.progression.TryGet(Data.Character.AP, out var ap))
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
            foreach (var allocatable in character.growth.Origin.Fields)
            {
                if (!character.growth.TryGet(allocatable.key, out var growth)) continue;
                var fd = Instantiate(fieldDisplayPrefab, contentRoot);
                fieldDisplayList.Add(fd);
                fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0 && allowDecreasePoints);
                fd.Bind(growth, allowDecreasePoints, true, (diff) =>
                {
                    ap.Value -= diff;
                    fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0 && allowDecreasePoints);
                }, f => { return $"{allocatable.Value} + {f.Value}"; });
                handles.Add(ap.Subscribe((l, l2) =>
                {
                    fd.SetButtonInteractable(l2 > 0, growth.Value > 0 && allowDecreasePoints);
                }));
            }
        }
    }
}