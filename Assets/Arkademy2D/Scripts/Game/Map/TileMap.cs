using UnityEngine;
using UnityEngine.Events;

namespace Arkademy2D.Game.Map
{
    public class TileMap : MonoBehaviour
    {
        public Transform entry;
        public UnityEvent onPlayerEnter;
    }
}