using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class WheatCollision : MonoBehaviour
{
    [SerializeField] Tilemap wheatTileMap;
    [SerializeField] GridLayout grid;
    [SerializeField] Tile wheatTile;
    [SerializeField] Tile dirtTile;

    public static void CheckTileAtCoordinate(Vector2 worldPosition)
    {
        
    }

    /// <summary>
    /// Checks if a wheat tile is present at the provided coordinate
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public bool IsWheatTilePresent(Vector2 worldPosition)
    {
        Vector3Int tilePos = grid.WorldToCell(worldPosition);
        if(wheatTileMap.GetTile<Tile>(tilePos) != null){ return true; }
        return false;
    }

    public void DeleteWheatTileAtCoordinate(Vector2 worldPosition)
    {
        Vector3Int tilePos = grid.WorldToCell(worldPosition);
        wheatTileMap.SetTile(tilePos, null);
    }
}
