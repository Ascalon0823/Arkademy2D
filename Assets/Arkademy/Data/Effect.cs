using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Effect
    {
        public List<Modifier> modifiers = new List<Modifier>();
    }
}