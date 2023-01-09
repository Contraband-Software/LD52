using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture.Managers;

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


        // Start is called before the first frame update
        void Start()
        {

            //disables the continue button if no level to
            //continue from
            if(GameController.Instance.Level == 0)
            {
                continueButton.SetActive(false);
            }

            blackOverlay.blocksRaycasts = false;
            blackOverlay.alpha = 0f;
        }


        public void ContinueGame()
        {
            //Load next level, disable scene activation
            GameController.Instance.ContinueGame();
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = false;

            //
            blackOverlay.blocksRaycasts = true;
            blackOverlayAnim.Play("FadeInBlack");
        }

        public void NewGame()
        {
            //Load next level, disable scene activation
            GameController.Instance.StartNewGame();
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = false;
            //
            blackOverlay.blocksRaycasts = true;
            blackOverlayAnim.Play("FadeInBlack");
        }

        public void FadeInBlackCompleted()
        {
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = true;
        }
    }
}
