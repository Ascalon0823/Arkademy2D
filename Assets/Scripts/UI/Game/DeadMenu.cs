using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class DeadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        private void Start()
        {
            menu.SetActive(false);
        }

        private void Update()
        {
            if (!PlayerBehaviour.Player) return;
            if (!PlayerBehaviour.Player.playerCharacter) return;
            if (PlayerBehaviour.Player.playerCharacter.isDead == menu.activeSelf) return;
            menu.SetActive(PlayerBehaviour.Player.playerCharacter.isDead);
        }
    }
}