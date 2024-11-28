using UnityEngine;

namespace Arkademy.Behaviour
{
    public class FollowMotorFixedTime : FollowMotor
    {
        public float usingTime;
        public float currTime;
        public Vector2 origin;

        public void UpdatePosition(float t)
        {
            transform.position = Vector3.Lerp(origin, target.position, t);
        }
        public override void FixedUpdate()
        {
            if (!target) return;
            var diff = (target.position - transform.position);
            if (rotateToMovDir) transform.up = diff.normalized;
            currTime += Time.fixedDeltaTime;
            var t = currTime / usingTime;
            transform.position = Vector3.Lerp(origin, target.position, t);
            if (t >= 1f)
            {
                OnReachingTarget?.Invoke(target);
            }
        }
    }
}