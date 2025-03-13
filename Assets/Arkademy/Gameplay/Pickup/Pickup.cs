using System;
using System.Collections;
using UnityEngine;

namespace Arkademy.Gameplay.Pickup
{
    public abstract class PickupBase : MonoBehaviour
    {
        public Collider2D trigger;
        public SpriteRenderer graphic;
        public float spawnDelay;
        public bool spawnAnimation;
        public bool pickupAnimation;
        public float pickupAnimationTime;
        public Character pickupCharacter;

        protected virtual void Awake()
        {
            Invoke("Ready", spawnDelay);
            if (spawnAnimation)
            {
                LeanTween.moveLocalY(graphic.gameObject, 1, (spawnDelay - 0.1f) / 2).setEaseOutCirc()
                    .setLoopPingPong(1);
                LeanTween.rotateLocal(graphic.gameObject, new Vector3(0, 0, 360 * 3), (spawnDelay - 0.1f));
            }
        }


        protected virtual void Ready()
        {
            trigger.enabled = true;
        }

        public virtual void PickupBy(Character character)
        {
            if (pickupCharacter) return;
            if (!CanBePickupBy(character)) return;
            pickupCharacter = character;
            if (pickupAnimation)
            {
                StartCoroutine(PickupAnimation());
                return;
            }

            if (Payload())
            {
                Destroy(gameObject);
                return;
            }

            pickupCharacter = null;
        }

        protected virtual IEnumerator PickupAnimation()
        {
            var curr = 0f;
            var originalPos = transform.position;
            pickupAnimationTime = Mathf.Clamp01(Vector3.Distance(originalPos, pickupCharacter.transform.position) / 5f);
            while (curr < pickupAnimationTime)
            {
                transform.position = Vector3.Lerp(originalPos, pickupCharacter.transform.position,
                    curr / pickupAnimationTime);
                curr += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (Payload())
            {
                Destroy(gameObject);
                yield break;
            }

            pickupCharacter = null;
        }


        protected abstract bool CanBePickupBy(Character character);
        protected abstract bool Payload();
    }
}