using UnityEngine;

namespace Arkademy.Backend
{
    [CreateAssetMenu(fileName = "Backend Environment", menuName = "Backend/Env", order = 0)]
    public class BackendEnvironment : ScriptableObject
    {
        public string envName;
        public string baseURL;
        public string overrideToken;
    }
}