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
        [SerializeField] GameObject rockGameObject;
        [SerializeField] RectTransform rockSpawnArea;

        [Header("Settings")]
        [SerializeField, Range(0, 0.01f)] float spawnChancePerTileRow = 0.001f;

        void Awake()
        {
            Vector4 bounds = new Vector4(
                Mathf.FloorToInt(rockSpawnArea.localPosition.x),
                Mathf.FloorToInt(rockSpawnArea.localPosition.y),
                Mathf.FloorToInt(rockSpawnArea.localPosition.x + rockSpawnArea.sizeDelta.x),
                Mathf.FloorToInt(rockSpawnArea.localPosition.y + rockSpawnArea.sizeDelta.y)
            );

            for (int y = (int)bounds.y; y < (int)bounds.w; y++)
            {
                for (int x = (int)bounds.x; x < (int)bounds.z; x++)
                {
                    if (Random.value > 1 - spawnChancePerTileRow)
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

            GameObject gObject = Instantiate(rockGameObject, rockTilemap.transform);
            gObject.transform.localPosition = Backend.Utilities.Mult_CWise((Vector3)position - new Vector3(0, 1, 0), tilemap.cellSize);
        }
    }
}