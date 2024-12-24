using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class CharacterAI : MonoBehaviour
    {
        public Character character;
        public void Update()
        {
            UseAbility();
        }

        public void UseAbility()
        {
            foreach (var ability in character.abilities)
            {
                if (ability.CanUse()) ability.Use();
            }
        }
    }
}