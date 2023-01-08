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
        [SerializeField] RectTransform timeLeftBar;

        private void Start()
        {
            PauseController.GetReference().PauseEvent.AddListener(ShowPauseMenu);
        }

        private void ShowPauseMenu()
        {
            
        }

        #region HUD_UPDATING
        public void SetPercentageTotal(float percentage)
        {
            percentageDisplayTotal.text = Mathf.Ceil(percentage).ToString() + "%";
        }

        public void UpdatePercentageHarvested(float percentage)
        {
            percentageDisplayProgress.text = Mathf.Floor(percentage).ToString();
        }
        public void UpdateTimeLeft(float timeLeftPercent)
        {
            Vector3 s = timeLeftBar.localScale;
            s.y = timeLeftPercent;
            timeLeftBar.localScale = s;
        }
        #endregion
    }
}