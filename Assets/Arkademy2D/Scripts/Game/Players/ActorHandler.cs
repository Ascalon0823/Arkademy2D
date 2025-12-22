using System;
using Arkademy2D.Game.System;
using Arkademy2D.Game.Actor;
using Arkademy2D.Game.Data.Runtime;
using UnityEngine;

namespace Arkademy2D.Game.Players
{
    public class ActorHandler : MonoBehaviour
    {
        public Health actorHealth;
        public Movement actorMovement;
        public Graphic actorGraphic;
        public CharacterData characterData;

        public void SetupUseCharacterData(CharacterData newCharacterData)
        {
            characterData = newCharacterData;
            ReloadCharacterActor();
        }

        private void Start()
        {
            actorHealth.onDamage.AddListener(() => { actorGraphic.SetAnimationTrigger("hit"); });
            if (characterData != null)
            {
                SetupUseCharacterData(characterData);
            }
        }

        public void ReloadCharacterActor()
        {
            actorHealth.max = characterData.CharacterModel.MaxHealth;
            actorHealth.current = actorHealth.max;
            actorMovement.speed = characterData.CharacterModel.MoveSpeed;
        }

        private void Update()
        {
            characterData.Update(Time.deltaTime);
        }
    }
}