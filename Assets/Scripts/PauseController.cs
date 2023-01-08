using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Architecture.Managers;

namespace Architecture.Managers {
    public class PauseController : MonoBehaviour
    {
        public UnityEvent PauseEvent { get; private set; } = new UnityEvent();

        public static PauseController GetReference()
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<PauseController>();
        }
        private void Start()
        {
            InputHandler.GetReference().KeyPressed_Escape.AddListener(PauseTheGame);
        }

        private void PauseTheGame()
        {
            Time.timeScale = 0f;

            PauseEvent.Invoke();
        }
    }


}

