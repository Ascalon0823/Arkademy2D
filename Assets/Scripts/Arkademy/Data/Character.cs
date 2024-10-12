using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Character
    {
        public List<Attribute> attributes;
    }

    [Serializable]
    public class Enemy : Character
    {
        public int xpDrop;
    }
}