using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Static;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Arkademy2D.Game.Behaviours
{
    public class ProceduralMapController : MapController
    {
        [SerializeField] protected int roomCount;
        [SerializeField] protected int roomSize;
        [SerializeField] protected Tilemap floor;
        [SerializeField] protected Tilemap wall;
        [SerializeField] protected TileBase floorTile;
        [SerializeField] protected TileBase wallTile;

        [SerializeField] protected Noise.CellularAutomataProvider provider;
        [SerializeField] protected int seed;
        [SerializeField] protected CharacterBase enemyBase;
        [SerializeField] protected Character enemyPrefab;
        [SerializeField] protected int maxPackPerRoom;
        [SerializeField] protected int maxEnemiesPerPack;

        public override void Setup()
        {
            base.Setup();
            var rooms = new List<Vector2Int>();
            floor.ClearAllTiles();
            wall.ClearAllTiles();
            Random.InitState(seed);
            var floorPoses = new List<Vector3Int>();
            var floorTiles = new List<TileBase>();
            var wallPoses = new List<Vector3Int>();
            var wallTiles = new List<TileBase>();
            var dirs = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left
            };
            for (var i = 0; i < roomCount; i++)
            {
                var set = false;
                while (!set)
                {
                    var curr = rooms.Count == 0
                        ? Vector2Int.zero
                        : (rooms[Random.Range(0, rooms.Count)] + dirs[Random.Range(0, dirs.Length)]);
                    if (rooms.Contains(curr))
                    {
                        continue;
                    }

                    rooms.Add(curr);
                    set = true;
                }
            }

            foreach (var room in rooms)
            {
                var preData = new float?[roomSize, roomSize];
                for (var i = 0; i < roomSize; i++)
                for (var j = 0; j < roomSize; j++)
                {
                    if (i == 0 || j == 0 || i == roomSize - 1 || j == roomSize - 1)
                    {
                        preData[i, j] = 1;
                    }
                }

                var openings = dirs.Where(x => rooms.Contains(x + room)).ToList();
                foreach (var opening in openings)
                {
                    var from = Vector2Int.one * roomSize / 2;
                    var to = from + opening * roomSize / 2;
                    var fromMin = from - Vector2Int.one * 2;
                    var fromMax = from + Vector2Int.one * 2;
                    var toMin = to - Vector2Int.one * 2;
                    var toMax = to + Vector2Int.one * 2;
                    for (var i = Mathf.Min(fromMin.x, toMin.x); i < Mathf.Max(fromMax.x, toMax.x); i++)
                    for (var j = Mathf.Min(fromMin.y, toMin.y); j < Mathf.Max(fromMax.y, toMax.y); j++)
                    {
                        if (i < 0 || j < 0 || i >= roomSize || j >= roomSize) continue;
                        preData[i, j] = 0;
                    }
                }

                for (var i = 0; i < Random.Range(2, maxPackPerRoom); i++)
                {
                    var center = Vector2Int.one * roomSize / 2;
                    var off = Random.insideUnitCircle * roomSize / 3f;
                    var offInt = new Vector2Int(Mathf.FloorToInt(off.x), Mathf.FloorToInt(off.y));
                    var packPos = offInt + center;
                    for(var j=-2;j<=2;j++)
                    for (var k = -2; k <= 2; k++)
                    {
                        preData[j+packPos.x, k+packPos.y] = 0;
                    }

                    for (var j = 0; j < Random.Range(3, maxEnemiesPerPack); j++)
                    {
                        var enemy = Instantiate(enemyPrefab, transform);
                        enemy.@base = enemyBase;
                        enemy.faction = 1;
                        var worldPos = room * roomSize + off + Random.insideUnitCircle * 2f;
                        enemy.Setup();
                        enemy.SetPosition(worldPos);
                        var control = enemy.AddComponent<AICharacterController>();
                        control.character = enemy;
                        control.targetDetectionRange = 5;
                    }
                }

                var data = provider.GetData(roomSize, roomSize, Random.Range(int.MinValue, int.MaxValue), preData);
                for (var i = 0; i < roomSize; i++)
                for (var j = 0; j < roomSize; j++)
                {
                    var tile = data[i, j] == 0f ? floorTile : wallTile;
                    var poses = data[i, j] == 0f ? floorPoses : wallPoses;
                    var tiles = data[i, j] == 0f ? floorTiles : wallTiles;
                    tiles.Add(tile);
                    poses.Add(new Vector3Int(i + room.x * roomSize - roomSize / 2, j + room.y * roomSize - roomSize / 2,
                        0));
                }
            }

            floor.SetTiles(floorPoses.ToArray(), floorTiles.ToArray());
            wall.SetTiles(wallPoses.ToArray(), wallTiles.ToArray());
        }
    }
}