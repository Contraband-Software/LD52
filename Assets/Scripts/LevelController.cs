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

        float timeLeft;

        [Header("Progress")]
        [SerializeField] int totalWheatOnField;

        private void Start()
        {
            timeLeft = timeLimitSeconds;
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;

            UIController.GetReference().UpdateTimeLeft(timeLeft);
            UIController.GetReference().UpdatePercentageHarvested(Wheat.WheatFieldManager.GetReference().GetPercentageHarvested());
        }

        public void SetTotalWheat(int w)
        {
            totalWheatOnField = w;
        }
    }
}