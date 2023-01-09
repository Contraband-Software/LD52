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
            LevelController.GetReference().GameOverEvent.AddListener(GameOver);

            pauseCanvas.enabled = false;

            StartCoroutine(FadeBloodMask());
        }

        private void GameOver(LevelController.GameOverReason reason)
        {
            StopAllCoroutines();
            switch (reason)
            {
                case LevelController.GameOverReason.Success_100Percent:
                case LevelController.GameOverReason.Success_RequiredWheat:
                    GameController.Instance.CompleteLevel();
                    break;
                case LevelController.GameOverReason.Fail_Time:
                    GameController.Instance.FailLevel();
                    break;
                case LevelController.GameOverReason.Fail_HarvesterExploded:
                    GameController.Instance.FailLevel();
                    break;
            }
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
        public void ResetBloodMask()
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

        IEnumerator FadeBloodMask()
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