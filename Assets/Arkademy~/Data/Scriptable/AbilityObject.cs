using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data.Scriptable
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Data/Ability", order = 0)]
    public class AbilityObject : ScriptableObject
    {
       
        public Ability ability;
        
    }
}