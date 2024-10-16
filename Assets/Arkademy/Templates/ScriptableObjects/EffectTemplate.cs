using UnityEngine;

namespace Arkademy.Templates{
    [CreateAssetMenu(fileName = "New Effect Template", menuName = "Template/Effect", order = 0)]
    public class EffectTemplate : ScriptableObject{
        public string effectName;
        public string description;
        public Sprite icon;
        public float duration;
        public float cooldown;
        public float damage;
        
    }
}