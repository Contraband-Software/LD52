using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour
    {
        public enum GameOverReason
        {
            Time,
            HarvesterExploded,
        }

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

        float timeLeft;

        private void Awake()
        {
            SoundSystem.Instance.PlayMusic("Game");
            Harvester.HarvesterController.GetReference().HarvesterDestroyed.AddListener(() =>
            {
                GameOverEvent.Invoke(GameOverReason.HarvesterExploded);
            });
        }

        private void Start()
        {
            timeLeft = timeLimitSeconds;
            UIControllerLevel.GetReference().SetPercentageTotal(percentageGoal);
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0f)
            {
                timeLeft = 0f;
                TimeRanOut();
            }
            UIControllerLevel.GetReference().UpdateTimeLeft(timeLeft / timeLimitSeconds);
            UIControllerLevel.GetReference().UpdatePercentageHarvested(Wheat.WheatFieldManager.GetReference().GetPercentageHarvested());
        }

        void TimeRanOut()
        {
            GameOverEvent.Invoke(GameOverReason.Time);
        }
    }
}