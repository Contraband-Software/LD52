using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public sealed class GameController : Backend.AbstractSingleton<GameController>
    {
        public int Level { get; private set; } = 0;
        public AsyncOperation SceneLoadingOperation { get; private set; } = null;

        [Header("Settings")]
        [Tooltip("The offset of the first level from the MainMenu scene, this allows you to put non-level scenes before the big batch of levels which sould come after everything else")]
        [SerializeField, Min(0)] int gameLevel1Offset = 0;
        [SerializeField] string mainMenuSceneName = "MainMenu";
        [SerializeField] string endOfLevelsSceneName = "MainMenu";

        protected override void SingletonAwake()
        {
            //Unused but required by Backend.AbstractSingleton<T>
        }

        private void LoadCurrentLevel()
        {
            SceneLoadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + Level + gameLevel1Offset);
            SceneLoadingOperation.allowSceneActivation = true;
        }

        #region GAME_LEVEL_INTERFACE
        public void CompleteLevel()
        {
#if UNITY_EDITOR
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == mainMenuSceneName || sceneName == endOfLevelsSceneName)
            {
                throw new System.InvalidOperationException("GameController: You can only use this function in a game level");
            }
#endif

            if (Level++ == SceneManager.sceneCountInBuildSettings)
            {
                SceneLoadingOperation = SceneManager.LoadSceneAsync(endOfLevelsSceneName);
            } else
            {
                LoadCurrentLevel();
            }
        }

        public void LeaveLevel()
        {
            SceneLoadingOperation = SceneManager.LoadSceneAsync(mainMenuSceneName);
            SceneLoadingOperation.allowSceneActivation = true;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion

        #region MAIN_MENU_INTERFACE
        public void ContinueGame()
        {
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
            {
                throw new System.InvalidOperationException("GameController: You can only use this function in the main menu");
            }
#endif
            LoadCurrentLevel();
        }

        public void StartNewGame()
        {
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
            {
                throw new System.InvalidOperationException("GameController: You can only use this function in the main menu");
            }
#endif
            Level = 0;
            ContinueGame();
        }

        public bool HasProgressed()
        {
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
            {
                throw new System.InvalidOperationException("GameController: You can only use this function in the main menu");
            }
#endif
            return Level != 0;
        }
        #endregion
    }
}