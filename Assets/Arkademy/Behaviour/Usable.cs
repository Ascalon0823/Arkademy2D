using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Usable : MonoBehaviour
    {
        public Character user;
        public float nextUseTime;
        public float useTime;

        public virtual bool Use()
        {
            if (nextUseTime > 0f) return false;
            nextUseTime = useTime;
            return true;
        }

        protected virtual void Update()
        {
            if (nextUseTime > 0) nextUseTime -= Time.deltaTime;
        }
    }
}