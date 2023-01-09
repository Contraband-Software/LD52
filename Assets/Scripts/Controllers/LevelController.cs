using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour
    {
        public enum GameOverReason
        {
            Success_100Percent,
            Success_RequiredWheat,

            Fail_Time,
            Fail_HarvesterExploded,
        }

        bool gameOver = false;

        #region EVENTS
        public sealed class GameOverEventType : UnityEvent<GameOverReason> { }
        public GameOverEventType GameOverEvent { get; private set; } = new GameOverEventType();
        #endregion

        public static LevelController GetReference()
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        }

        [Header("Settings")]
        [SerializeField, Min(0)] int timeLimitSeconds = 120;
        [SerializeField, Min(40)] int percentageGoal = 40;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] bool noFail = false;
#endif

        float timeLeft;

        private void Awake()
        {
            Harvester.HarvesterController.GetReference().HarvesterDestroyed.AddListener(() =>
            {
                HaltGame();
                GameOverEvent.Invoke(GameOverReason.Fail_HarvesterExploded);
            });
        }

        private void Start()
        {
            timeLeft = timeLimitSeconds;
            UIControllerLevel.GetReference().SetPercentageTotal(percentageGoal);
        }

        private void HaltGame()
        {
            gameOver = true;
            Harvester.HarvesterController.GetReference().LockControls(true);
        }

        void Update()
        {
            if (!gameOver)
            {
                UIControllerLevel.GetReference().UpdateTimeLeft(timeLeft / timeLimitSeconds);
                UIControllerLevel.GetReference().UpdatePercentageHarvested(Wheat.WheatFieldManager.GetReference().GetPercentageHarvested());

#if UNITY_EDITOR
                if (noFail)
                {
                    return;
                }
#endif
                if (timeLeft <= 0f)
                {
                    HaltGame();
                    timeLeft = 0f;

                    if (Mathf.RoundToInt(Wheat.WheatFieldManager.GetReference().GetPercentageHarvested()) == 100)
                    {
                        GameOverEvent.Invoke(GameOverReason.Success_100Percent);
                    }
                    else if (Wheat.WheatFieldManager.GetReference().GetPercentageHarvested() >= percentageGoal)
                    {
                        GameOverEvent.Invoke(GameOverReason.Success_RequiredWheat);
                    }
                    else
                    {
                        GameOverEvent.Invoke(GameOverReason.Fail_Time);
                    }
                }
                else
                {
                    timeLeft -= Time.deltaTime;
                }
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}