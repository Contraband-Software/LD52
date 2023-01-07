using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Tilemap))
    ]
    public class WheatCollision : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GridLayout grid;
        [SerializeField] Tile harvestedWheatTile;

        Tilemap wheatTileMap;

        private void Awake()
        {
            wheatTileMap = GetComponent<Tilemap>();

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
            if (wheatTileMap.GetTile<TileBase>(tilePos) != null) { return true; }
            return false;
        }

        public void DeleteWheatTileAtCoordinate(Vector2 worldPosition)
        {
            Vector3Int tilePos = grid.WorldToCell(worldPosition);
            wheatTileMap.SetTile(tilePos, null);
        }
    }
}