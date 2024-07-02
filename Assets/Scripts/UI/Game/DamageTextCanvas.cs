using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class DamageTextCanvas : MonoBehaviour
    {
        public static DamageTextCanvas Canvas;

        private Dictionary<Transform, DamageTextGroup> _spawnedGroups = new();
        [SerializeField] private DamageTextGroup groupPrefab;

        private void Awake()
        {
            if (Canvas && Canvas != this)
            {
                Destroy(gameObject);
                return;
            }

            Canvas = this;
        }

        public static void AddTextTo(Transform t, int text)
        {
            Canvas.InternalAddTextTo(t, text);
        }

        private void InternalAddTextTo(Transform t, int text)
        {
            if (!_spawnedGroups.TryGetValue(t, out var group)
                || !group || group.spawnedText.Count >= group.childLimit)
            {
                group = Instantiate(groupPrefab, transform);
                group.followTarget = t;
                _spawnedGroups[t] = group;
            }

            group.AddText(text);
        }
    }
}