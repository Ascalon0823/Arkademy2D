using Arkademy2D.Game.Data.Static;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
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
        public Transform entry;
        public bool setupCompleted;
        public virtual void Setup()
        {
            if (setupCompleted) return;
            setupCompleted = true;
        }
        

        public void GoTo(MapBase nextMap)
        {
            var next = Load(nextMap.id);
            next.Setup();
            Player.Local.character.SetPosition(next.entry.position);
        }
    }
}