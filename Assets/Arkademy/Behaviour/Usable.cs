using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Usable : MonoBehaviour
    {
        public Character user;
        public float nextUseTime;
        public float useTime;

        public virtual bool CanUse()
        {
            return nextUseTime <= 0;
        }
        public virtual void Use()
        {
            if (!CanUse()) return;
            nextUseTime = useTime;
        }

        protected virtual void Update()
        {
            if (nextUseTime > 0) nextUseTime -= Time.deltaTime;
        }
    }
}