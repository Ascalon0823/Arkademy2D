using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D body;

        public Vector2 moveDir;
        public Vector2 faceDir;

        public float speed;
        public Health health;

        public void FixedUpdate()
        {
            if (health && health.current <= 0) return;
            var movement = speed * Time.fixedDeltaTime * moveDir;
            body.MovePosition(body.position + movement);
            if (movement.sqrMagnitude > float.Epsilon)
            {
                faceDir = movement.normalized;
            }
        }
    }
}