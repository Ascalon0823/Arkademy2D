using UnityEngine;
using UnityEngine.Tilemaps;

namespace WangTile
{
    [CreateAssetMenu(fileName = "New Wang Tile", menuName = "Wang Tile", order = 0)]
    public class WangTile : ScriptableObject
    {
        public TileBase[] tiles = new TileBase[16];
    }
}