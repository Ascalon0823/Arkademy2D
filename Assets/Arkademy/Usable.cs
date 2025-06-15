using UnityEngine;

namespace Arkademy
{
    public class Usable : MonoBehaviour
    {
        public float useTime;
        public float remainingUseTime;

        public virtual void Use(Vector2 position)
        {
            remainingUseTime = useTime;
        }

        public virtual bool CanUse(Vector2 position)
        {
            return remainingUseTime <= 0 && isActiveAndEnabled;
        }
        protected virtual void Update()
        {
            if (remainingUseTime <= 0) return;
            remainingUseTime -= Time.deltaTime;
        }
    }
}