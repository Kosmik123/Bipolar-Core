using UnityEngine;
using UnityEditor;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar.Editor
{
    public class ApplicationSettingsWindow : EditorWindow
    {
        private int targetFrameRate;

        [MenuItem("Window/Application Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<ApplicationSettingsWindow>("Application Settings");
            window.Refresh();
        }

        private void OnGUI()
        {
            targetFrameRate = EditorGUILayout.IntField("Target Framerate", targetFrameRate);
            targetFrameRate = Mathf.Max(targetFrameRate, 0);
            Application.targetFrameRate = targetFrameRate;
        }

        public void Refresh()
        {
            targetFrameRate = Application.targetFrameRate;
        }
    }
}
