using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Managers
{
    public class BadEndingController : MonoBehaviour
    {
        private void Awake()
        {
            SoundSystem.Instance.StopAllSounds();
            SoundSystem.Instance.StopMusic();
        }

        public void BackToMainMenu()
        {
            GameController.Instance.LeaveLevel();
            GameController.Instance.ResetProgress();
        }
    }
}
