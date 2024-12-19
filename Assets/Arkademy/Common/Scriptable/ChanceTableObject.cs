using UnityEngine;

namespace Arkademy.Common
{
    [CreateAssetMenu(fileName = "New chance table", menuName = "Data/Chance Table", order = 0)]
    public class ChanceTableObject : ScriptableObject
    {
        public ChanceTable<GameObject> table;
    }
}