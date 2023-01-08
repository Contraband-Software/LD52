using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public class UIControllerLevel : MonoBehaviour
    {
        public static UIControllerLevel GetReference()
        {
            return GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIControllerLevel>();
        }

        [SerializeField] TextMeshProUGUI percentageDisplayProgress;
        [SerializeField] TextMeshProUGUI percentageDisplayTotal;
        [SerializeField] TextMeshProUGUI timeLeftDisplay;

        public void SetPercentageTotal(float percentage)
        {
            percentageDisplayTotal.text = percentage.ToString() + "%";
        }

        public void UpdatePercentageHarvested(float percentage)
        {
            percentageDisplayProgress.text = percentage.ToString();
        }
        public void UpdateTimeLeft(float timeLeft)
        {
            timeLeftDisplay.text = timeLeft.ToString();
        }
    }
}