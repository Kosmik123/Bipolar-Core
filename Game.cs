using Codice.Client.Common;
using UnityEngine;

namespace Bipolar
{
	internal class Game : ScriptableObject
    {
        public void Quit()
        {
            if (Application.isEditor == false)
            {
                Application.Quit();
            }
#if UNITY_EDITOR
            else
            {
                UnityEditor.EditorApplication.ExitPlaymode();
            }
#endif
        }
    }
}
