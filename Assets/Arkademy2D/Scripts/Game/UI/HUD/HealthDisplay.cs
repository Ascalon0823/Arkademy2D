using Arkademy2D.Game.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI.HUD
{
    public class HealthDisplay : MonoBehaviour
    {
        public Image fill;

        private void LateUpdate()
        {
            fill.fillAmount = (Player.Local.character.hp * 1f / Player.Local.character.maxHp.Value) * 0.8f +
                              0.15f;
        }
    }
}