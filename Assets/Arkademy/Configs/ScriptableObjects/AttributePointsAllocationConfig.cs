using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Configs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New AP Allocation Config", menuName = "Config/AP Allocation Config", order = 0)]
    public class AttributePointsAllocationConfig : ScriptableObject
    {
        public string freeAPKey;
        public List<string> allocatableAttributes;
    }
}