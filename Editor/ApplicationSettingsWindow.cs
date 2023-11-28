using UnityEngine;
using UnityEditor;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

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
            GUILayout.Label("Application Settings", EditorStyles.boldLabel);
            GUILayout.Space(10);

            int targetFrameRate = Application.targetFrameRate;
            targetFrameRate = EditorGUILayout.IntField("Target Framerate", targetFrameRate);
            targetFrameRate = Mathf.Max(targetFrameRate, 0);
            Application.targetFrameRate = targetFrameRate;

            float timeScale = Time.timeScale;
            timeScale = EditorGUILayout.FloatField("Time Scale", timeScale);
            timeScale = Mathf.Max(timeScale, 0);
            Time.timeScale = timeScale;
        }

        public void Refresh()
        {
        }


    }
}
