using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleStop : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }
}
