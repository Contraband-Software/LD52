using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Architecture.Managers
{
    [DisallowMultipleComponent]
    public class UIController : MonoBehaviour
    {
        public static UIController GetReference()
        {
            return GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIController>();
        }

        [SerializeField] TextMeshProUGUI percentageDisplay;
        [SerializeField] TextMeshProUGUI timeLeftDisplay;

        public void UpdatePercentageHarvested(float percentage)
        {
            percentageDisplay.text = percentage.ToString();
        }
        public void UpdateTimeLeft(float timeLeft)
        {
            timeLeftDisplay.text = timeLeft.ToString();
        }
    }
}