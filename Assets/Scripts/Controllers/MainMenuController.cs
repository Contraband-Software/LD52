using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture.Managers;
using UnityEngine.UI;

namespace Architecture.Managers
{
    public class MainMenuController : MonoBehaviour
    {
        public static MainMenuController GetReference()
        {
            return GameObject.FindGameObjectWithTag("MainMenuController").GetComponent<MainMenuController>();
        }

        [Header("UI Elements")]
        [SerializeField] GameObject continueButton;
        [SerializeField] CanvasGroup blackOverlay;
        [SerializeField] Animator blackOverlayAnim;
        [SerializeField] Image backgroundImage;

        [Header("Other referecnes")]
        [SerializeField] Sprite foggyBackground;


        // Start is called before the first frame update
        void Start()
        {

            //disables the continue button if no level to
            //continue from
            if(GameController.Instance.Level == 0)
            {
                continueButton.SetActive(false);
            }
            else
            {
                backgroundImage.sprite = foggyBackground;
            }

            blackOverlay.blocksRaycasts = false;
            blackOverlay.alpha = 0f;
        }


        public void ContinueGame()
        {
            FadeInBlackOverlay();

            //Load next level, disable scene activation
            GameController.Instance.ContinueGame();
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = false;
        }

        public void NewGame()
        {
            FadeInBlackOverlay();

            //Load next level, disable scene activation
            GameController.Instance.StartNewGame();
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = false;
        }

        private void FadeInBlackOverlay()
        {
            blackOverlay.blocksRaycasts = true;
            blackOverlayAnim.Play("FadeInBlack");
        }

        public void FadeInBlackCompleted()
        {
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = true;
        }
    }
}
