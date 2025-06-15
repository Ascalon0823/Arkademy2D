using System.Collections;
using UnityEngine;

namespace Arkademy
{
    public class SpriteDamagedFX : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Color original;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            GetComponent<Damageable>().onDamageTakenAt.AddListener(OnDamaged);
            original = sr.color;
        }

        public void OnDamaged(int damage, Vector2 position)
        {
            sr.color = new Color(1f, sr.color.g, sr.color.b, sr.color.a);
            LeanTween.color(gameObject, original, 0.1f)
                .setOnUpdateColor(c =>
                {
                    sr.color = c;
                }).setEaseOutQuad();
        }
    }
}