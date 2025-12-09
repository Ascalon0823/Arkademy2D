using System;
using Arkademy2D.Core.System;
using Arkademy2D.Game.Actors;
using UnityEngine;

namespace Arkademy2D.Game.Player
{
    public class ActorHandler : MonoBehaviour
    {
        public Health actorHealth;

        private void Start()
        {
            actorHealth.max = GameSystem.CharacterData.MaxHealth;
            actorHealth.current = actorHealth.max;
        }

        public void TakeDamage(int damage)
        {
            actorHealth.current -= damage;
            actorHealth.current =  Mathf.Clamp(actorHealth.current, 0, actorHealth.max);
        }
    }
}