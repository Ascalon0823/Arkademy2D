using System;
using Arkademy.Common;
using UnityEngine;
using UnityEngine.UI;
using Attribute = Arkademy.Common.Attribute;
using Character = Arkademy.Gameplay.Character;

namespace Arkademy.UI
{
    public class CharacterResourcesBar : MonoBehaviour
    {
        public Image bar;
        public Image bg;
        public float bgSpeed;
        public Resource targetResource;
        public Attribute.Type attributeType;
        public Character character;
        private void Start()
        {
            switch(attributeType)
            {
                case Attribute.Type.None:
                    break;
                case Attribute.Type.Energy:
                    break;
                case Attribute.Type.Health:
                    targetResource = character.characterData.health;
                    break;
                case Attribute.Type.Source:
                    targetResource = character.characterData.source;
                    break;
                case Attribute.Type.Speed:
                    break;
                case Attribute.Type.CastSpeed:
                    break;
                case Attribute.Type.Attack:
                    break;
                case Attribute.Type.Defence:
                    break;
                case Attribute.Type.DetectionRange:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (targetResource.value == 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            bar.fillAmount = targetResource.currValue * 1f / targetResource.value;
            var target = Mathf.Lerp(bg.fillAmount, bar.fillAmount, Time.deltaTime * bgSpeed);
            if (Mathf.Abs(target - bar.fillAmount) < 0.1f * Time.deltaTime)
            {
                target = bar.fillAmount;
            }
            bg.fillAmount = target;
            
        }
    }
}