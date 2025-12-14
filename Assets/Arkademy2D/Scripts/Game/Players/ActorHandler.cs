using System;
using Arkademy2D.Core.System;
using Arkademy2D.Game.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Player
{
    public class ActorHandler : MonoBehaviour
    {
        public Health actorHealth;
        public Movement actorMovement;
        public Graphic actorGraphic;
        private void Start()
        {
            actorHealth.onDamage.AddListener(() => { actorGraphic.SetAnimationTrigger("hit"); });
            ReloadCharacterActor();
        }

        public void ReloadCharacterActor()
        {
            actorHealth.max = GameSystem.Character.CharacterModel.MaxHealth;
            actorHealth.current = actorHealth.max;
            actorMovement.speed = GameSystem.Character.CharacterModel.MoveSpeed;
        }
    }
}