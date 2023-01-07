using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Tilemap))
    ]
    public class WheatMap : MonoBehaviour
    {
        [Header("Edge Tiles")]
        [SerializeField] Tile topEdge;
        [SerializeField] Tile bottomEdge;
        [SerializeField] Tile leftEdge;
        [SerializeField] Tile rightEdge;

        [Header("Corner Tiles")]
        [SerializeField] Tile topLeftCornerLarge;
        [SerializeField] Tile topRightCornerLarge;
        [SerializeField] Tile bottomLeftCornerLarge;
        [SerializeField] Tile bottomRightCornerLarge;

        [SerializeField] Tile topLeftCornerSmall;
        [SerializeField] Tile topRightCornerSmall;
        [SerializeField] Tile bottomLeftCornerSmall;
        [SerializeField] Tile bottomRightCornerSmall;

        Tilemap wheatTilemap;

    //    private sealed class LocalTile
    //    {
    //        public Vector3Int position { get; set; }
    //        public Tile tile { get; set; }
    //    }

    //    LocalTile[] localTiles = new LocalTile[9];

    //    enum TileType
    //    {
    //        N_Edge,
    //        W_Edge, E_Edge,
    //        S_Edge,

    //        NW_LargeCorner, NE_LargeCorner,
    //        SW_LargeCorner, SE_LargeCorner,

    //        NW_SmallCorner, NE_SmallCorner,
    //        SW_SmallCorner, SE_SmallCorner
    //    }

    //    private void Awake()
    //    {
    //        wheatTilemap = GetComponent<Tilemap>();
    //    }

    //    private TileType GetTileType(Tile tile)
    //    {
    //        if (tile == topEdge) { return TileType.N_Edge; }
    //        if (tile == bottomEdge) { return TileType.S_Edge; }
    //        if (tile == leftEdge) { return TileType.W_Edge; }
    //        if (tile == rightEdge) { return TileType.E_Edge; }

    //        if (tile == topLeftCornerLarge) { return TileType.NW_LargeCorner; }
    //        if (tile == topRightCornerLarge) { return TileType.NE_LargeCorner; }
    //        if (tile == bottomLeftCornerLarge) { return TileType.SW_LargeCorner; }
    //        if (tile == bottomRightCornerLarge) { return TileType.SE_LargeCorner; }

    //        if (tile == topLeftCornerSmall) { return TileType.NW_SmallCorner; }
    //        if (tile == topRightCornerSmall) { return TileType.NE_SmallCorner; }
    //        if (tile == bottomLeftCornerSmall) { return TileType.SW_SmallCorner; }
    //        if (tile == bottomRightCornerSmall) { return TileType.SE_SmallCorner; }

    //        throw new System.ArgumentException("Invalid Tile");
    //    }

    //    private void GetLocalTiles(Vector3Int position)
    //    {
    //        #region TOP_LEFT_TILE
    //        localTiles[0].position = position + new Vector3Int(-1, -1);
    //        localTiles[0].tile = wheatTilemap.GetTile<Tile>(localTiles[0].position);
    //        #endregion

    //        #region TOP_MIDDLE_TILE
    //        localTiles[1].position = position + new Vector3Int(0, -1);
    //        localTiles[1].tile = wheatTilemap.GetTile<Tile>(localTiles[1].position);
    //        #endregion

    //        #region TOP_RIGHT_TILE
    //        localTiles[2].position = position + new Vector3Int(1, -1);
    //        localTiles[2].tile = wheatTilemap.GetTile<Tile>(localTiles[2].position);
    //        #endregion


    //        #region MIDDLE_LEFT_TILE
    //        localTiles[3].position = position + new Vector3Int(-1, 0);
    //        localTiles[3].tile = wheatTilemap.GetTile<Tile>(localTiles[3].position);
    //        #endregion

    //        localTiles[4] = null;

    //        #region MIDDLE_RIGHT_TILE
    //        localTiles[5].position = position + new Vector3Int(1, 0);
    //        localTiles[5].tile = wheatTilemap.GetTile<Tile>(localTiles[5].position);
    //        #endregion


    //        #region BOTTOM_LEFT_TILE
    //        localTiles[6].position = position + new Vector3Int(-1, 1);
    //        localTiles[6].tile = wheatTilemap.GetTile<Tile>(localTiles[6].position);
    //        #endregion

    //        #region BOTTOM_MIDDLE_TILE
    //        localTiles[7].position = position + new Vector3Int(0, 1);
    //        localTiles[7].tile = wheatTilemap.GetTile<Tile>(localTiles[7].position);
    //        #endregion

    //        #region BOTTOM_RIGHT_TILE
    //        localTiles[8].position = position + new Vector3Int(1, 1);
    //        localTiles[8].tile = wheatTilemap.GetTile<Tile>(localTiles[8].position);
    //        #endregion
    //    }

    //    public void DestoryTile(uint x, uint y)
    //    {
    //        GetLocalTiles(new Vector3Int((int)x, (int)y));

    //        switch (GetTileType(tile))
    //        {
    //            case TileType.N_Edge:
    //                break;
    //            case TileType.W_Edge:
    //                break;
    //            case TileType.E_Edge:
    //                break;
    //            case TileType.S_Edge:
    //                break;

    //            case TileType.NW_LargeCorner:
    //                break;
    //            case TileType.NE_LargeCorner:
    //                break;
    //            case TileType.SW_LargeCorner:
    //                break;
    //            case TileType.SE_LargeCorner:
    //                break;

    //            case TileType.NW_SmallCorner:
    //                break;
    //            case TileType.NE_SmallCorner:
    //                break;
    //            case TileType.SW_SmallCorner:
    //                break;
    //            case TileType.SE_SmallCorner:
    //                break;
    //        }
    //    }
    }
}