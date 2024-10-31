using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Behaviour
{
    public class FollowMotor : Motor
    {
        public Transform target;
        public UnityEvent<Transform> OnReachingTarget;
        public float followThresh;

        public override void FixedUpdate()
        {
            if (!target) return;
            var diff = (target.position - transform.position);
            if (rotateToMovDir) transform.up = diff.normalized;
            if (diff.magnitude < followThresh)
            {
                OnReachingTarget?.Invoke(target);
                return;
            }
            transform.position += Vector3.ClampMagnitude(Time.fixedDeltaTime * moveSpeed
                                                                             * diff.normalized, diff.magnitude);
        }
    }
}