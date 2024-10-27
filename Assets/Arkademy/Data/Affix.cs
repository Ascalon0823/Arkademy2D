using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Affix
    {
        public List<Effect> effects;
        [Range(0f,1f)]
        public float value;
    }
}   