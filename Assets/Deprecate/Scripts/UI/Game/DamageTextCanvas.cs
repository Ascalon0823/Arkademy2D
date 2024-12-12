using System;
using System.Collections;
using System.Collections.Generic;
using Arkademy.Behaviour.UI;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class DamageTextCanvas : MonoBehaviour
    {
        public static DamageTextCanvas Canvas;

        [SerializeField] private DamageText textPrefab;
        // private Dictionary<Transform, DamageTextGroup> _spawnedGroups = new();
        // [SerializeField] private DamageTextGroup groupPrefab;

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

        public static void AddTextTo(Transform t, Data.DamageEvent de)
        {
            Canvas.StartCoroutine(Canvas.SpawnTextCoroutine(t, de));
        }

        private void InternalAddTextTo(Transform t, DamageEvent de)
        {
            // if (!_spawnedGroups.TryGetValue(t, out var group)
            //     || !group || group.dealerInstance != de.dealerInstance
            //     || group.batch != de.batch)
            // {
            //     group = Instantiate(groupPrefab, transform);
            //     group.followTarget = t;
            //     _spawnedGroups[t] = group;
            // }
            //
            // group.AddText(de);
        }

        IEnumerator SpawnTextCoroutine(Transform t, Data.DamageEvent de)
        {
            var interval = 1f / de.damages.Length;
            interval = Mathf.Min(0.1f, interval);
            DamageText prev = null;
            for (var i = 0; i < de.damages.Length; i++)
            {
                var cam = Behaviour.Game.localPlayers[0].characterCamera.ppcam.cam;
                var beginningPos = prev
                    ? prev.beginningWorldPos +
                      new Vector3(0, 0.35f)
                    : (t.position + new Vector3(0, 0.6f));
                var text = Instantiate(textPrefab, cam.WorldToScreenPoint(beginningPos), Quaternion.identity, transform);
                text.beginningWorldPos = beginningPos;
                text.cam = cam;
                text.content = de.damages[i].ToString();
                prev = text;
                yield return new WaitForSeconds(interval);
            }
        }
    }
}