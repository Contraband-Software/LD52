using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Architecture.Managers;

public class WheatCounter : MonoBehaviour
{
    [SerializeField] Tilemap wheatField;
    LevelController levelController;
    // Start is called before the first frame update
    void Start()
    {
        levelController = LevelController.GetReference();
        wheatField = GetComponent<Tilemap>();

        CountAllWheatOnMap();
    }
    /// <summary>
    /// Counts all the wheat tiles on the attached tilemap
    /// and relays the number to the level controller
    /// </summary>
    private void CountAllWheatOnMap()
    {
        BoundsInt bounds = wheatField.cellBounds;
        TileBase[] allTiles = wheatField.GetTilesBlock(bounds);

        int numberOfWheat = allTiles.Length;
        levelController.SetTotalWheat(numberOfWheat);
    }

    
}
