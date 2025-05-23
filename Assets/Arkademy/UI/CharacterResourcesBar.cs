using System;
using UnityEngine;
using UnityEngine.UI;
using Attribute = Arkademy.Data.Attribute;
using Character = Arkademy.Gameplay.Character;

namespace Arkademy.UI
{
    public class CharacterResourcesBar : MonoBehaviour
    {
        public Image bar;
        public Image bg;
        public float bgSpeed;
        public Attribute.Type attributeType;
        public Character character;
        public CanvasGroup group;
        public bool hideIfEmpty;
        public bool hideIfFull;

        public void Update()
        {
            group.alpha = character.isDead ? 0 : 1;
            bar.fillAmount = character.Attributes.GetCurr(attributeType) / Mathf.Max(character.Attributes.Get(attributeType),1f);
            var target = Mathf.Lerp(bg.fillAmount, bar.fillAmount, Time.deltaTime * bgSpeed);
            if (Mathf.Abs(target - bar.fillAmount) < 0.1f * Time.deltaTime)
            {
                target = bar.fillAmount;
            }
            bg.fillAmount = target;
        }
    }
}