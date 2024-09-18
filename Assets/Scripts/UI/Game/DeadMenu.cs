using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class DeadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private TextMeshProUGUI deadMessage;

        private void Start()
        {
            menu.SetActive(false);
        }

        private void Update()
        {
            if (!Player.Chara) return;
            if (Player.Curr.playerCharacter.isDead == menu.activeSelf) return;
            menu.SetActive(Player.Curr.playerCharacter.isDead);
            Player.Curr.pauseCount += 1;
            deadMessage.text = $"You have survived for {StageBehaviour.Current.secondsPlayed:N0} seconds";
        }
    }
}