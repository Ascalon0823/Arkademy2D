using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class AbilityPayload : MonoBehaviour
    {
        public AbilityBase ability;
        public virtual void Init(AbilityEventData data, AbilityBase parent)
        {
            ability = parent;
        }

        public virtual void UpdatePayload(AbilityEventData data)
        {
            
        }

        public virtual void Cancel()
        {
            
        }
    }
}