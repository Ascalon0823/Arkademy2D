using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public static class Extensions
    {
        public static void SetLayerRecursive(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursive(layer);
            }
        }

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
        
        private static readonly Dictionary<Collider2D, Gameplay.Character> ColliderCharacterMap = new Dictionary<Collider2D, Gameplay.Character>();

        public static bool GetCharacter(this Collider2D collider, out Gameplay.Character character)
        {
            return ColliderCharacterMap.TryGetValue(collider, out character);
        }

        public static void RegisterCharacterCollider(this Collider2D collider, Gameplay.Character character)
        {
            ColliderCharacterMap[collider] =  character;
        }
    }
}