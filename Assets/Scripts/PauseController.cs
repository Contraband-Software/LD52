using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Architecture.Managers;

namespace Architecture.Managers {
    [
        RequireComponent(typeof(LevelController))
    ]

    public class PauseController : MonoBehaviour
    {
        public UnityEvent PauseEvent { get; private set; } = new UnityEvent();

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
            CheckForInputs();
        }

        private void CheckForInputs()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseTheGame();
            }
        }

        private void PauseTheGame()
        {
            PauseEvent.Invoke();
        }
    }


}

