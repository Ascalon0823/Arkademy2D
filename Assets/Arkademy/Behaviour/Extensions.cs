using UnityEngine;

namespace Arkademy.Behaviour
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var ret = go.GetComponent<T>();
            if (!ret) ret = go.AddComponent<T>();
            return ret;
        }
        public static T GetOrAddComponent<T>(this Component c) where T : Component
        {
            return c.gameObject.GetOrAddComponent<T>();
        }
    }
}