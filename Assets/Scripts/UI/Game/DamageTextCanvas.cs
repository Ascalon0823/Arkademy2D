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

        public static void AddTextTo(Transform t, DamageEvent de)
        {
            Canvas.InternalAddTextTo(t, de);
        }

        private void InternalAddTextTo(Transform t, DamageEvent de)
        {
            if (!_spawnedGroups.TryGetValue(t, out var group)
                || !group || group.dealerInstance != de.dealerInstance
                || group.batch != de.batch)
            {
                group = Instantiate(groupPrefab, transform);
                group.followTarget = t;
                _spawnedGroups[t] = group;
            }

            group.AddText(de);
        }
    }
}