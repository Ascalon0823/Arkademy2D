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
        }
        public Character GetNewCharacter()
        {
            return templateData.Copy();
        }
    }
}