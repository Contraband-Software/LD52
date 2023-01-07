using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladePFXController : MonoBehaviour
{
    //This script will find place a particle effect
    //where grass was cut, by selecting a PFX not currently playing

    [SerializeField] List<BladePFX> pfxList = new List<BladePFX>();

    /// <summary>
    /// Plays a harvesting effect at the point where wheat was cut
    /// </summary>
    public void PlayHarvestPFX(Vector2 wheatPosition)
    {
        foreach(BladePFX bpfx in pfxList)
        {
            if (!bpfx.currentPlayingPFX)
            {
                bpfx.PlayPFX(wheatPosition);
                break;
            }
        }
    }
    
}
