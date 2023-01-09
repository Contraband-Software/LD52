using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture.Managers;

namespace Architecture.Managers
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] GameObject continueButton;
        [SerializeField] CanvasGroup blackOverlay;


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
            GameController.Instance.SceneLoadingOperation.allowSceneActivation = false;
            GameController.Instance.ContinueGame();

            //
            blackOverlay.blocksRaycasts = true;
        }
    }
}
