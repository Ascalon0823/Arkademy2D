using System;
using Arkademy2D.Core.System;
using Arkademy2D.Game.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Player
{
    public class ActorHandler : MonoBehaviour
    {
        public Health actorHealth;
        public Graphic actorGraphic;
        private void Start()
        {
            actorHealth.max = GameSystem.CharacterData.MaxHealth;
            actorHealth.current = actorHealth.max;
            actorHealth.onDamage.AddListener(() => { actorGraphic.SetAnimationTrigger("hit"); });
        }
        
    }
}