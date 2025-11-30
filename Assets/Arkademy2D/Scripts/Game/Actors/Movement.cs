using UnityEngine;

namespace Arkademy2D.Game.Actors
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D body;

        public Vector2 moveDir;

        public float speed;

        public void FixedUpdate()
        {
            var movement = speed * Time.fixedDeltaTime * moveDir;
            body.MovePosition(body.position + movement);
        }
    }
}