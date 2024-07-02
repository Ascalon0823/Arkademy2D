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
    }
}