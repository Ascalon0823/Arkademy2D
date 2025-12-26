using System.Collections.Generic;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class Caster : MonoBehaviour
    {
        public List<string> castKeys;
        public bool casting;
        public void Cast(string key)
        {
            if (castKeys == null)
            {
                castKeys = new List<string>();
            }

            if (!castKeys.Contains(key))
            {
                castKeys.Add(key);
            }
        }

        public void EndCast()
        {
            if (castKeys == null) return;
            Debug.Log($"End cast with spell key {string.Join('-',castKeys)}");
            castKeys = null;
        }
    }
}