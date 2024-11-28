using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class CharacterMenu : MonoBehaviour
    {
        public Character character;
        public CanvasGroup group;

        public Image characterSprite;
        public FillBar lifeFillBar;
        public FillBar resourceBar;
        public Player player;
        public NamedPages details;
        private void Start()
        {
            player = GetComponentInParent<Player>();
        }

        private void Update()
        {
            character = player.controllingCharacter;
            group.interactable = character;
            group.alpha = character ? 1 : 0;
            group.blocksRaycasts = character;
            if (!character)
            {
                return;
            }
            
        }

        public void ToggleCharacterDetails(bool active)
        {
            details.Toggle(active);
        }
    }
}