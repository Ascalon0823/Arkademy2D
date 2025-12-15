using Arkademy2D.Game.System;
using Arkademy2D.Game.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Players
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