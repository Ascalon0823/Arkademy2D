using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Character Template", menuName = "Template/Character", order = 0)]
    public class CharacterTemplate : ScriptableObject
    {
        public Character templateData;
        public Behaviour.Character usingPrefab;
        [Header("Graphics")]
        public Sprite characterSprite;
        public RuntimeAnimatorController animationController;
        public float walkAnimationDistance;
        public bool facingRight;
        public bool useForCharaCreation;
        private void OnEnable()
        {
            templateData.templateName = name;
            var chara = GetNewCharacter();
            if (chara.progression.TryGet("Level", out var value))
            {
                value.Value = 2;
            }

            if (chara.growth.TryGet("Strength", out var str))
            {
                str.Value = 1;
            }

            var s = JsonConvert.SerializeObject(chara);
            Debug.Log(s);
            Debug.Log(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Character>(s)));
        }
        public Character GetNewCharacter()
        {
            return templateData.Copy();
        }
    }
}