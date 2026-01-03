using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public abstract class Usage : MonoBehaviour
    {
        [SerializeField] protected Usable fromUsable;

        public virtual void Init(Usable usable)
        {
            fromUsable = usable;
        }
    }
}