using System;
using UnityEngine;

namespace Bipolar
{
    internal class DebugLogger : ScriptableObject
    {
        public void Log(string message) => Log(message, Debug.Log);
        public void LogError(string message) => Log(message, Debug.LogError);
        public void LogWarning(string message) => Log(message, Debug.LogWarning);

        private static void Log(string message, Action<string> logMethod) => logMethod.Invoke(message);
    }
}
