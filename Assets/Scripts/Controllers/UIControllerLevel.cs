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
        public enum State
        {
            Playing,
            Paused,
            LevelFail,
            LevelComplete
        }

        public State UIState { get ; private set; } = State.Playing;
        public string GameOverMessage { get; private set; } = "";

        public static UIControllerLevel GetReference()
        {
            return GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIControllerLevel>();
        }

        [Header("References")]
        [SerializeField] TextMeshProUGUI percentageDisplayProgress;
        [SerializeField] TextMeshProUGUI percentageDisplayTotal;
        [SerializeField] RectTransform timeLeftBar;
        [SerializeField] Image bloodMaskImage;
        [Header("Canvas References")]
        [SerializeField] Canvas pauseCanvas;
        [SerializeField] Canvas levelCompleteCanvas;
        [SerializeField] Canvas levelFailCanvas;

        [Header("Settings")]
        [SerializeField, Range(0, 0.1f)] float bloodMaskFadeSpeed = 0.05f;

        float currentMaskOpacity = 0;

        private void Start()
        {
            PauseController.GetReference().PauseEvent.AddListener(() => { SetState(State.Paused); });
            PauseController.GetReference().UnPauseEvent.AddListener(() => { SetState(State.Playing); });
            LevelController.GetReference().GameOverEvent.AddListener(GameOver);

            SetState(State.Playing);

            StartCoroutine(FadeBloodMask());
        }

        private void GameOver(LevelController.GameOverReason reason)
        {
            StopAllCoroutines();
            switch (reason)
            {
                case LevelController.GameOverReason.Success_100Percent:
                    GameOverMessage = "You harvested 100% of the wheat, well done!";
                    SetState(State.LevelComplete);
                    break;
                case LevelController.GameOverReason.Success_RequiredWheat:
                    GameOverMessage = "You harvested enough wheat in time.";
                    SetState(State.LevelComplete);
                    break;
                case LevelController.GameOverReason.Fail_Time:
                    GameOverMessage = "You didn't harvest enough wheat in time!";
                    SetState(State.LevelFail);
                    break;
                case LevelController.GameOverReason.Fail_HarvesterExploded:
                    GameOverMessage = "You hit a rock and blew up.";
                    SetState(State.LevelFail);
                    break;
            }
        }

        private void SetState(State state)
        {
            UIState = state;
            switch (state)
            {
                case State.Playing:
                    pauseCanvas.enabled = false;
                    levelFailCanvas.enabled = false;
                    levelCompleteCanvas.enabled = false;
                    break;
                case State.Paused:
                    pauseCanvas.enabled = true;
                    levelFailCanvas.enabled = false;
                    levelCompleteCanvas.enabled = false;
                    break;
                case State.LevelFail:
                    pauseCanvas.enabled = false;
                    levelFailCanvas.enabled = true;
                    levelCompleteCanvas.enabled = false;
                    break;
                case State.LevelComplete:
                    pauseCanvas.enabled = false;
                    levelFailCanvas.enabled = false;
                    levelCompleteCanvas.enabled = true;
                    break;
            }
        }

        public void Pause()
        {
            PauseController.GetReference().PauseTheGame();
        }

        public void UnPause()
        {
            PauseController.GetReference().UnPauseTheGame();
        }

        public void Restart()
        {
            UnPause();
            Debug.Log("Rest");
            GameController.Instance.Restart();
        }

        public void Leave()
        {
            UnPause();
            GameController.Instance.LeaveLevel();
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