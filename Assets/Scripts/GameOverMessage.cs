using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Architecture
{
    [
        RequireComponent(typeof(TextMeshProUGUI)),
        DisallowMultipleComponent
    ]
    public class GameOverMessage : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<TextMeshProUGUI>().text = Managers.UIControllerLevel.GetReference().GameOverMessage;
        }
    }
}