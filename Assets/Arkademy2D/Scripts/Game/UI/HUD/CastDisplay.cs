using System.Collections.Generic;
using Arkademy2D.Game.Behaviours;
using UnityEngine;

namespace Arkademy2D.Game.UI.HUD
{
    public class CastDisplay : MonoBehaviour
    {
        public List<SpriteRenderer> castKeyImage;
        public Transform holder;
        private void LateUpdate()
        {
            var caster = Player.Local.character;
            if (!caster) return;
            holder.gameObject.SetActive(caster.showCasting);
            if (!caster.showCasting) return;
            foreach (var keyImage in castKeyImage)
            {
                keyImage.color = caster.castKeys!=null && caster.castKeys.Contains(keyImage.gameObject.name) ? Color.white : Color.gray;
            }
        }
    }
}