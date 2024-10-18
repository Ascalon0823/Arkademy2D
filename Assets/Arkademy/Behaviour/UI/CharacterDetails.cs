using System;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class CharacterDetails : MonoBehaviour
    {
        private void Start()
        {
            Toggle(false);
        }

        public void Toggle(bool active)
        {
            Game.RequirePause(active);
            gameObject.SetActive(active);
        }

        public void UpdatePage(int diff)
        {
        }
    }
}