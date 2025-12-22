using Arkademy2D.Game.Behaviours;
using Arkademy2D.Game.Behaviours.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    public struct UseContext
    {
        public Transform userTransform;
        public Character character;
        public Vector2 direction;
    }
}