using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Grid)),
        DisallowMultipleComponent
    ]
    public class WheatFieldManager : MonoBehaviour
    {
        public static WheatFieldManager GetReference()
        {
            return GameObject.FindGameObjectWithTag("Map").GetComponent<WheatFieldManager>();
        }

        [Header("References")]
        [SerializeField] Tilemap wheatTilemap;
        [SerializeField] Tilemap harvestedWheatTilemap;
        [SerializeField] Tile harvestedWheatTile;

        Grid grid;

        uint amountOfWheat = 0;
        uint wheatHarvested = 0;

        private void Awake()
        {
            grid = GetComponent<Grid>();

            BoundsInt bounds = wheatTilemap.cellBounds;
            TileBase[] allTiles = wheatTilemap.GetTilesBlock(bounds);

            for (int y = 0; y < bounds.size.y; y++)
            {
                for (int x = 0; x < bounds.size.x; x++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        amountOfWheat++;
                    }
                }
            }

            Debug.Log(amountOfWheat);

#if UNITY_EDITOR
            if (harvestedWheatTile == null)
            {
                throw new System.ArgumentException("No harvested wheat tile reference");
            }
#endif
        }

        /// <summary>
        /// Checks if a wheat tile is present at the provided coordinate
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public bool IsWheatTilePresent(Vector2 worldPosition)
        {
            Vector3Int tilePos = grid.WorldToCell(worldPosition);
            if (wheatTilemap.GetTile<TileBase>(tilePos) != null) { return true; }
            return false;
        }

        public void DeleteWheatTileAtCoordinate(Vector2 worldPosition)
        {
            Vector3Int tilePos = grid.WorldToCell(worldPosition);
            wheatTilemap.SetTile(tilePos, null);
            harvestedWheatTilemap.SetTile(tilePos, harvestedWheatTile);
            wheatHarvested++;
        }

        public float GetPercentageHarvested()
        {
            return ((float)wheatHarvested / (float)amountOfWheat) * 100.0f;
        }
    }
}