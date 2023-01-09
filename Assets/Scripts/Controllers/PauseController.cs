using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Architecture.Managers;

namespace Architecture.Managers {
    [DisallowMultipleComponent]
    public class PauseController : MonoBehaviour
    {
        public UnityEvent PauseEvent { get; private set; } = new UnityEvent();
        public UnityEvent UnPauseEvent { get; private set; } = new UnityEvent();

        public bool GamePaused { get; private set; } = false;

        public static PauseController GetReference()
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<PauseController>();
        }
        private void Start()
        {
            InputHandler.GetReference().KeyPressed_Escape.AddListener(PauseTheGame);
        }

        public void PauseTheGame()
        {
            if (!GamePaused)
            {
                GamePaused = true;

                Time.timeScale = 0f;

                PauseEvent.Invoke();
            }
        }

        public void UnPauseTheGame()
        {
            Debug.Log(GamePaused);
            if (GamePaused)
            {
                GamePaused = false;

                Time.timeScale = 1f;

                UnPauseEvent.Invoke();
            }
        }
    }
}

