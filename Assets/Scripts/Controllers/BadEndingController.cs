using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Managers
{
    public class BadEndingController : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            GameController.Instance.LeaveLevel();
        }
    }
}
