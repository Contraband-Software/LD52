using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture.Managers;
[
    RequireComponent(typeof(LevelController))
]

public class PauseController : MonoBehaviour
{
    public static PauseController GetReference()
    {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<PauseController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckForInputs()
    {

    }
}
