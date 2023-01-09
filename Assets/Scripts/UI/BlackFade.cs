using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Architecture.Managers;

public class BlackFade : MonoBehaviour
{
    public void FadedToBlack()
    {
        MainMenuController.GetReference().FadeInBlackCompleted();
    }
}
