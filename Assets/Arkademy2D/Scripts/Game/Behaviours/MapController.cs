using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Interaction;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    [Serializable]
    public class MapConnection
    {
        public Interactable portal;
        public Transform entry;
        public MapBase destination;
    }
    public class MapController : MonoBehaviour
    {
        public static MapController Load(int mapBaseIdx)
        {
            var mapBase = MapBase.Get(mapBaseIdx);
            if (!mapBase) return null;
            return Load(mapBase);
        }

        public static MapController Load(MapBase mapBase)
        {
            if(_currInstance)Destroy(_currInstance.gameObject);
            _currInstance = Instantiate(mapBase.mapPrefab);
            return _currInstance;
        }

        private static MapController _currInstance;
        [SerializeField] protected MapBase @base;
        public Transform defaultSpawnPoint;
        public bool setupCompleted;
        public List<MapConnection> connections;
        public virtual void Setup()
        {
            if (setupCompleted) return;
            setupCompleted = true;
            SetupConnections();
        }

        public virtual void SetupConnections()
        {
            if (connections == null) return;
            foreach (var connection in connections)
            {
                connection.portal.onInteracted.AddListener(() =>
                {
                    GoTo(connection.destination);
                });
            }
        }

        public void GoTo(MapBase nextMap)
        {
            var next = Load(nextMap.id);
            next.Setup();
            var connection = next.connections.Where(x => x.destination == @base).FirstOrDefault();
            var nextEntryPos = connection == null ? GetDefaultSpawnPoint(): connection.entry.position;
            Player.Local.character.SetPosition(nextEntryPos);
        }

        public Vector3 GetDefaultSpawnPoint()
        {
            return defaultSpawnPoint?.position??Vector3.zero;
        }
    }
}