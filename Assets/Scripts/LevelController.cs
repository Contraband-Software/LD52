using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour
    {
        public static LevelController GetReference()
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        }

        [Header("Settings")]
        [SerializeField, Min(0)] int timeLimitSeconds = 120;
        [SerializeField, Min(40)] int percentageGoal = 40;

        float timeLeft;

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

        }
    }
}