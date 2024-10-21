using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class CharacterStatus : NamedPage
    {
        public Character character;
        public FieldDisplay charaName;
        public FieldDisplay level;
        public FieldDisplay xP;
        public FieldDisplay aP;

        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private FieldDisplay fieldDisplayPrefab;
        [SerializeField] private List<FieldDisplay> fieldDisplayList = new();

        public override void OnActivated(bool activated)
        {
            base.OnActivated(activated);
            Setup(Game.localPlayers[0].controllingCharacter);
        }

        public void Setup(Character newCharacter)
        {
            if (character == newCharacter) return;
            character = newCharacter;
            charaName.valueText.text = character.name;
            if (character.data.progression.TryGet(Data.Character.Lv, out var lv))
            {
                level.Bind(lv);
            }

            if (character.data.progression.TryGet(Data.Character.XP, out var xp))
            {
                xP.Bind(xp);
            }

            if (character.data.progression.TryGet(Data.Character.AP, out var ap))
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

            fieldDisplayList.Clear();
            foreach (var allocatable in character.data.growth.Origin.Fields)
            {
                if (!character.data.growth.TryGet(allocatable.key, out var growth)) continue;
                var fd = Instantiate(fieldDisplayPrefab, contentRoot);
                fieldDisplayList.Add(fd);
                fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0);
                fd.Bind(growth, true, true, (diff) =>
                {
                    ap.Value -= diff;
                    fd.SetButtonInteractable(ap.Value > 0, growth.Value > 0);
                }, f => { return $"{allocatable.Value} + {f.Value}"; });
                ap.OnValueChange += (l, l2) => { fd.SetButtonInteractable(l2 > 0, growth.Value > 0); };
            }
        }
    }
}