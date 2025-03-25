using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Rift
{
    public class RiftRoom
    {
        public RiftRoom[] Connected = new RiftRoom[4];
        public Vector2Int Position;
        public int[,] Cells;
        public int Seed;
        public int roomSize;
        public int openingSize;
        public List<Vector2Int> OpenArea = new List<Vector2Int>();
        public int iterations;
        public float percentage;
        public int minCondition;
        public int maxCondition;

        public Vector2Int GetRandomOpenArea()
        {
            return OpenArea[Random.Range(0, OpenArea.Count)];
        }

        public Vector3 WorldPos(Vector2Int coord)
        {
            var worldCoord =  Position * roomSize + coord;
            return new Vector3(worldCoord.x, worldCoord.y);
        }

        public void Automata()
        {
            for (var i = 0; i < iterations; i++)
            {
                var temp = new int[roomSize, roomSize];
                for (var x = 0; x < roomSize; x++)
                for (var y = 0; y < roomSize; y++)
                {
                    var neighbourCount = 0;
                    for (var k = 0; k < 8; k++)
                    {
                        var dir = RiftMap.Directions8[k];
                        var pos = dir + new Vector2Int(x, y);
                        if (pos.x < 0 || pos.x > roomSize - 1 || pos.y < 0 || pos.y > roomSize - 1 ||
                            Cells[pos.x, pos.y] == 1)
                        {
                            neighbourCount++;
                        }
                    }

                    temp[x, y] = neighbourCount >= maxCondition || neighbourCount<=minCondition ? 1 : 0;
                }

                Cells = temp;
            }
        }

        public void Fill()
        {
            Random.InitState(Seed);
            Cells = new int[roomSize, roomSize];
            for (var x = 0; x < roomSize; x++)
            for (var y = 0; y < roomSize; y++)
            {
                Cells[x, y] = Random.Range(0f, 1f) > percentage ? 0 : 1;
                Cells[x, y] = x == 0 || x == roomSize - 1 || y == 0 || y == roomSize - 1 ? 1 : Cells[x, y];
            }

            for (var dir = 0; dir < RiftMap.Directions.Count; dir++)
            {
                if (Connected[dir] == null) continue;
                var center = new Vector2Int(roomSize / 2, roomSize / 2);
                var off = roomSize / 2 * RiftMap.Directions[dir];
                var from = center + off - openingSize * Vector2Int.one;
                var to = center + off + openingSize * Vector2Int.one;
                for (var x = from.x; x <= to.x; x++)
                for (var y = from.y; y <= to.y; y++)
                {
                    if (x < 0 || x > roomSize - 1 || y < 0 || y > roomSize - 1) continue;
                    Cells[x, y] = 0;
                }
            }

            Automata();
            for (var x = 0; x < roomSize; x++)
            for (var y = 0; y < roomSize; y++)
            {
                if (Cells[x, y] == 1) continue;
                OpenArea.Add(new Vector2Int(x, y));
            }
        }
    }

    public class RiftMap : MonoBehaviour
    {
        public int seed;
        public int roomCount;

        public int roomSize;
        public int openingSize;

        public Vector3 startPos;

        [Range(0, 1f)] public float openPercentage;
        [Range(0, 10)] public int iterations;
        public bool preview;

        [Range(-1, 4)] public int minCondition;
        [Range(4, 9)] public int maxCondition;

        public static readonly List<Vector2Int> Directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static readonly List<Vector2Int> Directions8 = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right + Vector2Int.down,
            Vector2Int.down,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left + Vector2Int.up,
        };

        public Dictionary<Vector2Int, RiftRoom> Rooms = new Dictionary<Vector2Int, RiftRoom>();
        public Texture2D previewTexture;

        public void Generate()
        {
            Rooms.Clear();
            Random.InitState(seed);
            var pos = Vector2Int.zero;
            RiftRoom prev = null;
            for (var i = 0; i < roomCount; i++)
            {
                var room = new RiftRoom
                {
                    roomSize = roomSize,
                    openingSize = openingSize,
                    Position = pos,
                    Seed = Random.Range(int.MinValue, int.MaxValue),
                    iterations = iterations,
                    percentage = openPercentage,
                    minCondition = minCondition,
                    maxCondition = maxCondition
                };
                Rooms.Add(pos, room);
                if (prev != null)
                {
                    var dir = prev.Position - pos;
                    var dirIdx = Directions.IndexOf(dir);
                    room.Connected[dirIdx] = prev;
                    var oppositeIdx = Directions.IndexOf(-dir);
                    prev.Connected[oppositeIdx] = room;
                    room.Cells = new int[roomSize, roomSize];
                }

                prev = room;
                var candidate = Directions.Where(x => !Rooms.ContainsKey(x + pos)).ToList();
                if (!candidate.Any()) break;

                pos += candidate[Random.Range(0, candidate.Count)];
            }

            foreach (var room in Rooms.Values)
            {
                room.Fill();
            }

            var startRoom = Rooms[Vector2Int.zero];
            startPos = startRoom.WorldPos(startRoom.GetRandomOpenArea());
        }

        protected virtual void OnValidate()
        {
            if (!preview) return;
            if (!previewTexture)
            {
                previewTexture = new Texture2D(roomSize, roomSize);
            }

            previewTexture.Reinitialize(roomSize, roomSize);
            var data = new int[roomSize, roomSize];
            Random.InitState(seed);
            for (var x = 0; x < roomSize; x++)
            for (var y = 0; y < roomSize; y++)
            {
                data[x, y] = Random.Range(0f, 1f) > openPercentage ? 0 : 1;
                data[x, y] = x == 0 || x == roomSize - 1 || y == 0 || y == roomSize - 1 ? 1 : data[x, y];
            }

            for (var i = 0; i < iterations; i++)
            {
                var temp = new int[roomSize, roomSize];
                for (var x = 0; x < roomSize; x++)
                for (var y = 0; y < roomSize; y++)
                {
                    var neighbourCount = 0;
                    for (var k = 0; k < 8; k++)
                    {
                        var dir = RiftMap.Directions8[k];
                        var pos = dir + new Vector2Int(x, y);
                        if (pos.x < 0 || pos.x > roomSize - 1 || pos.y < 0 || pos.y > roomSize - 1 ||
                            data[pos.x, pos.y] == 1)
                        {
                            neighbourCount++;
                        }
                    }

                    temp[x, y] = neighbourCount >= maxCondition || neighbourCount <= minCondition ? 1 : 0;
                }

                data = temp;
            }

            for (var i = 0; i < roomSize; i++)
            for (var j = 0; j < roomSize; j++)
            {
                previewTexture.SetPixel(i, j, data[i, j] == 1 ? Color.black : Color.white);
            }

            previewTexture.Apply();
        }
    }
}