using System;
using Arkademy.Data;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Arkademy.Templates;

namespace Arkademy.Configs
{
    [CreateAssetMenu(fileName = "Affix Config", menuName = "Config/Affix Config", order = 0)]
    public class AffixConfig : ScriptableObject
    {
        public List<AffixTemplate> affixes;
    }
}