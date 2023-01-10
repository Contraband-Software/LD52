using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Architecture.Managers
{
    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Debug Abilities");

            if (GUILayout.Button("Win Level"))
            {
                LevelController.GetReference().GameOverEvent.Invoke(LevelController.GameOverReason.Success_100Percent);
            }
            if (GUILayout.Button("Skip Level"))
            {
                GameController.Instance.CompleteLevel();
                GameController.Instance.LoadCurrentLevel();
            }
            if (GUILayout.Button("Lose Level"))
            {
                LevelController.GetReference().GameOverEvent.Invoke(LevelController.GameOverReason.Fail_Time);
            }
        }
    }
}