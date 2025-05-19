using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Generators
{
    public static class TileSeedTracker
    {
        public static Dictionary<int, List<Vector3Int>> TilesBySeed = new();
    }
    public class LocationGenerator : IGenerator<LocationSeed>
    {
        private LocationSeed _locSeed;

        public LocationGenerator(LocationSeed locSeed)
        {
            _locSeed = locSeed;
        }

        public LocationSeed Generate()
        {
            Random.InitState(_locSeed.Seed);

            Vector2Int size = _locSeed.townSize;
            Vector2Int start = _locSeed.origin;
            Tile groundPrefab = _locSeed.groundPrefabs;
            Tile roadPrefab = _locSeed.roadPrefabs;

            Tilemap tilemap = GameObject.FindFirstObjectByType<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogWarning("Tilemap not found in scene.");
                return _locSeed;
            }

            GenerateGround();
            GenerateExits();

            Debug.Log($"Generated {_locSeed.locationName} at origin {start} with size {size}");
            return _locSeed;
            
            void GenerateGround()
            {
                if (!TileSeedTracker.TilesBySeed.TryGetValue(_locSeed.Seed, out List<Vector3Int> trackedTiles))
                {
                    trackedTiles = new List<Vector3Int>();
                    TileSeedTracker.TilesBySeed[_locSeed.Seed] = trackedTiles;
                }

                for (int x = 0; x <= size.x; x++)
                {
                    for (int y = 0; y <= size.y; y++)
                    {
                        Vector3Int pos = new Vector3Int(start.x + x, start.y + y, 0);

                        // Only place and track if it hasn't been placed before
                        if (!trackedTiles.Contains(pos))
                        {
                            tilemap.SetTile(pos, groundPrefab);
                            trackedTiles.Add(pos);
                        }
                    }
                }
            }

            void GenerateExits()
            {
                if (!TileSeedTracker.TilesBySeed.TryGetValue(_locSeed.Seed, out List<Vector3Int> trackedTiles))
                {
                    trackedTiles = new List<Vector3Int>();
                    TileSeedTracker.TilesBySeed[_locSeed.Seed] = trackedTiles;
                }

                int exits = Random.Range(4, 8);
                int xMid = size.x / 2;
                int yMid = size.y / 2;
                bool xLast = false;

                for (int i = 0; i < exits; i++)
                {
                    int xOffset = Random.Range(-xMid, xMid);
                    int yOffset = Random.Range(-yMid, yMid);

                    Vector3Int pos;

                    if (xLast)
                    {
                        pos = new Vector3Int(start.x + xMid + xOffset, start.y + size.y, 0); // just above town
                        xLast = false;
                    }
                    else
                    {
                        pos = new Vector3Int(start.x + size.x, start.y + yMid + yOffset, 0); // just right of town
                        xLast = true;
                    }

                    tilemap.SetTile(pos, roadPrefab);
                    trackedTiles.Add(pos);
                    Debug.Log($"Placing road exit at {pos}");
                }
            }

        }
    }

    [CreateAssetMenu(fileName = "NewLocationSeed", menuName = "WorldGen/Location")]
    public class LocationSeed : ScriptableObject
    {
        private const int MAX_SEED = 99999;

        [SerializeField, ReadOnly]
        private int seed;
        public int Seed => seed;

        public Vector2Int origin = Vector2Int.zero;
        public Vector2Int townSize = new Vector2Int(5, 5);
        public string locationName = "Townsville";
        public Tile groundPrefabs;
        public Tile roadPrefabs;

        private void OnEnable()
        {
            if (seed == 0)
            {
                seed = UnityEngine.Random.Range(1, MAX_SEED);
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        [Button("Generate")]
        public void Generate()
        {
            var generator = new LocationGenerator(this);
            generator.Generate();
        }

        [Button("Clear")]
        public void Clear()
        {
            Tilemap tilemap = GameObject.FindFirstObjectByType<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogWarning("Tilemap not found.");
                return;
            }

            if (TileSeedTracker.TilesBySeed.TryGetValue(seed, out List<Vector3Int> tiles))
            {
                foreach (var pos in tiles)
                {
                    tilemap.SetTile(pos, null);
                }

                Debug.Log($"Cleared all tiles placed with seed {seed}.");
                tiles.Clear(); // Optionally clear tracking
            }
            else
            {
                Debug.Log($"No tiles recorded for seed {seed}.");
            }
        }

        [Button("Randomize Seed")]
        private void RandomizeSeed()
        {
            if (seed != 0) return;
            seed = UnityEngine.Random.Range(1, MAX_SEED);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
    
}
