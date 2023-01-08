using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture
{
    [
        RequireComponent(typeof(Grid)),
        DisallowMultipleComponent
    ]
    public class RockSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Tilemap rockTilemap;
        [SerializeField] Tilemap wheatTileMap;
        [SerializeField] Tile[] rockTiles;

        [Header("Settings")]
        [SerializeField, Range(0, 1)] float spawnChance = 0.96f;

        void Awake()
        {
            BoundsInt bounds = rockTilemap.cellBounds;

            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
                {
                    if (Random.value > spawnChance)
                    {
                        PlaceRock(rockTilemap, new Vector3Int(x, y));
                        x++;
                        y++;
                    }
                }
            }
        }

        private void PlaceRock(Tilemap tilemap, Vector3Int position)
        {
            for (int i = 0; i < rockTiles.Length; i++) {
                Vector3Int pos = position + new Vector3Int(i % 2, -1 * Mathf.FloorToInt(i / 2.0f));
                tilemap.SetTile(pos, rockTiles[i]);
                wheatTileMap.SetTile(pos, null);
            }
        }
    }
}