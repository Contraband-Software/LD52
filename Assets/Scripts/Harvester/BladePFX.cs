using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class BladePFX : MonoBehaviour
{
    [SerializeField] ParticleSystem pfx;
    public bool currentPlayingPFX = false;

    void Start()
    {
        pfx = gameObject.GetComponent<ParticleSystem>();
    }

    public void PlayPFX(Vector2 positionToMoveTo)
    {

    }
}
