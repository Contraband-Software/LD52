using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Grid))
    ]
    public class WheatCollision : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Tilemap wheatTilemap;
        [SerializeField] Tilemap harvestedWheatTilemap;
        [SerializeField] Tile harvestedWheatTile;

        Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();

#if UNITY_EDITOR
            //if (harvestedWheatTile == null)
            //{
            //    throw new System.ArgumentException("No harvested wheat tile reference");
            //}
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
        }
    }
}