using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //[SerializeField] Tilemap groundTilemap;
        [SerializeField] Tilemap wheatTilemap;
        [SerializeField] Tilemap harvestedWheatTilemap;
        [SerializeField] Tile harvestedWheatTile;
        [SerializeField] RuleTile wheatTile;
        [SerializeField] RectTransform wheatSpawnArea;

        [Header("Generation Settings")]
        [Tooltip("Keep below 1.")]
        [SerializeField, Min(0)] float perlinScale = 1f;
        [SerializeField, Range(0, 1)] float perlinThreshold = 0.4f;
        //[SerializeField, Min(0)] uint chunks = 4;
        //[SerializeField, Range(0, 1)] float maxChunkSize = 0.8f;
        //[SerializeField, Range(0, 1)] float shiftFactor = 0.2f;

        Grid grid;

        uint amountOfWheat = 0;
        uint wheatHarvested = 0;

        private void Awake()
        {
            grid = GetComponent<Grid>();

#if UNITY_EDITOR
#pragma warning disable S112
            if (wheatTile == null)
            {
                throw new System.NullReferenceException("You must set the wheat tile reference");
            }
#pragma warning restore S112
#endif

            GenerateWheatField();

            BoundsInt bounds = wheatTilemap.cellBounds;
            Debug.Log("Wheat tile map: " + bounds.ToString());
            wheatTilemap.SetTile(new Vector3Int(-83, -7), wheatTile);
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
                {
                    if (wheatTilemap.GetTile<RuleTile>(new Vector3Int(x, y)) == wheatTile)
                    {
                        amountOfWheat++;
                    }
                }
            }

#if UNITY_EDITOR
            if (harvestedWheatTile == null)
            {
                throw new System.ArgumentException("No harvested wheat tile reference");
            }
#endif
        }

        /// <summary>
        /// x must be normalized.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        //private float PolynomialFalloff(float x)
        //{
        //    return 2f * 0.3f / (Mathf.Pow(x + 0.8f, 5));
        //}
        private void GenerateWheatField()
        {
            Vector4 bounds = new Vector4(
                Mathf.FloorToInt(wheatSpawnArea.localPosition.x),
                Mathf.FloorToInt(wheatSpawnArea.localPosition.y),
                Mathf.FloorToInt(wheatSpawnArea.localPosition.x + wheatSpawnArea.sizeDelta.x),
                Mathf.FloorToInt(wheatSpawnArea.localPosition.y + wheatSpawnArea.sizeDelta.y)
            );

            for (int y = (int)bounds.y; y < (int)bounds.w; y++)
            {
                for (int x = (int)bounds.x; x < (int)bounds.z; x++)
                {
                    if (Mathf.PerlinNoise(x / perlinScale, y / perlinScale) + Random.Range(0.0f, 0.3f) > perlinThreshold)
                    {
                        wheatTilemap.SetTile(new Vector3Int(x, y), wheatTile);
                    } else
                    {
                        wheatTilemap.SetTile(new Vector3Int(x, y), null);
                    }
                }
            }

            //for (int i = 0; i < chunks; i++)
            //{
            //    Debug.Log("#################################");
            //    int chunkX = Mathf.FloorToInt(Random.Range(groundTilemap.cellBounds.x, (groundTilemap.cellBounds.x + groundTilemap.cellBounds.size.x) * (1 - shiftFactor)));
            //    int chunkY = Mathf.FloorToInt(Random.Range(groundTilemap.cellBounds.y, (groundTilemap.cellBounds.y + groundTilemap.cellBounds.size.y) * (1 - shiftFactor)));
            //    int chunkWidth = Mathf.FloorToInt(PolynomialFalloff((float)i / (float)chunks) * maxChunkSize * groundTilemap.cellBounds.size.x);
            //    int chunkHeight = Mathf.FloorToInt(PolynomialFalloff((float)i / (float)chunks) * maxChunkSize * groundTilemap.cellBounds.size.y);

            //    Debug.Log("Position: " + new Vector2Int(chunkX, chunkY).ToString());
            //    Debug.Log("Size: " + new Vector2Int(chunkWidth, chunkHeight).ToString());
            //    Vector2Int finalSize = new Vector2Int(
            //        (int)Mathf.Clamp((float)(chunkX + chunkWidth), (float)(groundTilemap.cellBounds.x), (float)(groundTilemap.cellBounds.x + groundTilemap.cellBounds.size.x)),
            //        (int)Mathf.Clamp((float)(chunkY + chunkHeight), (float)(groundTilemap.cellBounds.y), (float)(groundTilemap.cellBounds.y + groundTilemap.cellBounds.size.y))
            //    );
            //    Debug.Log("Final size: " + finalSize.ToString());

            //    for (int y = chunkY; y < finalSize.y; y++)
            //    {
            //        for (int x = chunkX; x < finalSize.x; x++)
            //        {
            //            wheatTilemap.SetTile(new Vector3Int(x, y), wheatTile);
            //        }
            //    }
            //    Debug.Log("#################################");
            //}
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

            //Debug.Log(wheatHarvested.ToString() + " / " + amountOfWheat.ToString());

            Managers.SoundSystem.Instance.PlaySound("Wheat_Harvest", true);
        }

        public float GetPercentageHarvested()
        {
            return ((float)wheatHarvested / (float)amountOfWheat) * 100.0f;
        }
    }
}