using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Tilemap))
    ]
    public class WheatTint : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float perlinScale = 1;

        void Start()
        {
            Tilemap wheatField = GetComponent<Tilemap>();

            BoundsInt bounds = wheatField.cellBounds;
            TileBase[] allTiles = wheatField.GetTilesBlock(bounds);

            for (int y = 0; y < bounds.size.y; y++)
            {
                for (int x = 0; x < bounds.size.x; x++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        float factor = Mathf.Clamp(Mathf.PerlinNoise(x * perlinScale, y * perlinScale), 0.5f, 1);
                        RuleTile ruleTile = wheatField.GetTile<RuleTile>(new Vector3Int(x, y));
                        if (ruleTile)
                        {
                            //Debug.Log("meed");
                        }
                    }
                }
            }

            wheatField.RefreshAllTiles();
        }
    }
}