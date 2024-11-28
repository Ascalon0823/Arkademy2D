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

        private void LateUpdate()
        {
            var time = Session.currCharacterRecord.time;
            var energy = Session.currCharacterRecord.character.energy;
            timeText.text = $"{time.HourString()} {time.DayString()} {time.WeekString()}";
            energyText.text = $"{energy.currValue}/{energy.maxValue}";
        }
    }
}