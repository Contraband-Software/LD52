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

        [Header("References")]
        [SerializeField] TextMeshProUGUI percentageDisplayProgress;
        [SerializeField] TextMeshProUGUI percentageDisplayTotal;
        [SerializeField] RectTransform timeLeftBar;
        [SerializeField] Image bloodMaskImage;
        [SerializeField] Canvas pauseCanvas;

        [Header("Settings")]
        [SerializeField, Range(0, 0.1f)] float bloodMaskFadeSpeed = 0.05f;

        float currentMaskOpacity = 0;

        private void Start()
        {
            PauseController.GetReference().PauseEvent.AddListener(ShowPauseMenu);
            PauseController.GetReference().UnPauseEvent.AddListener(HidePauseMenu);

            pauseCanvas.enabled = false;

            StartCoroutine(FadeMask());
        }

        private void ShowPauseMenu()
        {
            pauseCanvas.enabled = true;
        }

        private void HidePauseMenu()
        {
            print("HIDING PAUSE MENU");
            pauseCanvas.enabled = false;
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

        #region BLOODMASK
        /// <summary>
        /// Removes the blood mask
        /// </summary>
        public void ResetMask()
        {
            currentMaskOpacity = 0;
        }

        /// <summary>
        /// Shows the blood mask and then fades it out over time
        /// </summary>
        public void ShowBloodMask()
        {
            currentMaskOpacity = 0.9994f;
        }

        IEnumerator FadeMask()
        {
            while (enabled)
            {
                currentMaskOpacity += (currentMaskOpacity - 1) * bloodMaskFadeSpeed;
                bloodMaskImage.color = new Color(
                    bloodMaskImage.color.r,
                    bloodMaskImage.color.g,
                    bloodMaskImage.color.b,
                    currentMaskOpacity
                );
                yield return new WaitForSeconds(0.01f);
            }
        }
        #endregion
    }
}