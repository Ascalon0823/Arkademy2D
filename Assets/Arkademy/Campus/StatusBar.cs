using System;
using System.Globalization;
using Arkademy.Common;
using TMPro;
using UnityEngine;

namespace Arkademy.Campus
{
    public class StatusBar : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI energyText;

        private void Start()
        {
            Session.currCharacterRecord.time.OnNewDay += OnNewDay;
        }

        private void OnDestroy()
        {
            Session.currCharacterRecord.time.OnNewDay -= OnNewDay;
        }

        private void OnNewDay(int day)
        {
            Session.currCharacterRecord.character.energy.currValue = Session.currCharacterRecord.character.energy.maxValue;
        }

        private void LateUpdate()
        {
            var time = Session.currCharacterRecord.time;
            var energy = Session.currCharacterRecord.character.energy;
            timeText.text = $"{time.HourString()} {time.DayString()} {time.WeekString()}";
            energyText.text = $"{energy.currValue}/{energy.maxValue}";
        }
    }
}