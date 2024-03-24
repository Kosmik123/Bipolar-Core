using UnityEngine;
using UnityEditor;

namespace Bipolar.Editor
{
    public class ApplicationSettingsWindow : EditorWindow
    {
        [MenuItem("Window/Application Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<ApplicationSettingsWindow>("Application Settings");
            window.Refresh();
        }

        private void OnGUI()
        {
            ShowBasicApplicationSettings();
            ShowCursorSettings();
        }

        private static void ShowBasicApplicationSettings()
        {
            GUILayout.Space(10);
            GUILayout.Label("Application Settings", EditorStyles.boldLabel);

            int targetFrameRate = Application.targetFrameRate;
            targetFrameRate = EditorGUILayout.IntField("Target Framerate", targetFrameRate);
            targetFrameRate = Mathf.Max(targetFrameRate, 0);
            Application.targetFrameRate = targetFrameRate;

            float timeScale = Time.timeScale;
            timeScale = EditorGUILayout.FloatField("Time Scale", timeScale);
            timeScale = Mathf.Max(timeScale, 0);
            Time.timeScale = timeScale;
        }


        private static void ShowCursorSettings()
        {
            GUILayout.Space(10);
            GUILayout.Label("Cursor", EditorStyles.boldLabel);

            bool isCursorVisible = Cursor.visible;
            isCursorVisible = EditorGUILayout.Toggle("Visible", isCursorVisible);
            Cursor.visible = isCursorVisible;

            var lockState = Cursor.lockState;
            lockState = (CursorLockMode)EditorGUILayout.EnumPopup("Lock State", lockState);
            Cursor.lockState = lockState;
        }


        public void Refresh()
        {

        }
    }
}
