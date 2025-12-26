using Arkademy2D.Game.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI.HUD
{
    public class EnergyDisplay : MonoBehaviour
    {
        public Image fill;

        private void LateUpdate()
        {
            fill.fillAmount = (Player.Local.character.energy.current * 1f / Player.Local.character.energy.max) * 0.8f +
                              0.15f;
        }
    }
}