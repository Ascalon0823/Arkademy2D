using System;
using Arkademy2D.Game.Behaviours;
using Arkademy2D.Game.Behaviours.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public struct UseContext
    {
        public Character character;
        public Vector2 point;
        public Usable usable;
    }
}