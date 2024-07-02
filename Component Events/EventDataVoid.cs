using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Prototyping.ComponentEvents
{
    [System.Serializable]
    internal abstract class AbstractEventData
    {
        public string eventName;
        public abstract UnityEventBase UnityEvent { get; }
        public EventInfo EventInfo { get; set; }
        public System.Delegate InvokeDelegate { get; set; }

        internal void Clear()
        {
            eventName = null;
            InvokeDelegate = null;
            EventInfo = null;
            UnityEvent.RemoveAllListeners();
        }
    }

    [System.Serializable]
    internal partial class EventDataVoid : AbstractEventData
    {
        [SerializeField]
        internal UnityEvent unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal partial class EventDataInt : AbstractEventData
    {
        [SerializeField]
        internal UnityEvent<int> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal partial class EventDataFloat : AbstractEventData
    {
        [SerializeField]
        internal UnityEvent<float> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal partial class EventDataString : AbstractEventData
    {
        [SerializeField]
        internal UnityEvent<string> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal partial class EventDataBool : AbstractEventData
    {
        [SerializeField]
        internal UnityEvent<bool> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

#if UNITY_EDITOR
    internal class ComponentEventsBuildPreprocessor : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            Debug.Log("Preprocess build");
            //PlayerSettings.SetAdditionalIl2CppArgs("--compilation-defines=HEJKA_MISKU");
            PlayerSettings.SetAdditionalIl2CppArgs("--compiler-flags=\"--compilation-defines=HEJKA_MISKU\"");
        }
    }
#endif
}


