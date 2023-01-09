using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Architecture.Managers;

public class BlackFade : MonoBehaviour
{
    
    public enum WorkingInScene { MAINMENU, GAME}
    public WorkingInScene scene;

    public void FadedToBlack()
    {
        if(scene == WorkingInScene.MAINMENU)
        {
            MainMenuController.GetReference().FadeInBlackCompleted();
        }
        if(scene == WorkingInScene.GAME)
        {
            UIControllerLevel.GetReference().FadeInBlackCompleted();
        }
    }
}
