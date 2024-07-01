using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Prototyping.ComponentEvents
{
    [System.Serializable]
    internal abstract class EventDataBase
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
    internal class EventData : EventDataBase
    {
        [SerializeField]
        internal UnityEvent unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal class EventDataInt : EventDataBase
    {
        [SerializeField]
        internal UnityEvent<int> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal class EventDataFloat : EventDataBase
    {
        [SerializeField]
        internal UnityEvent<float> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal class EventDataString : EventDataBase
    {
        [SerializeField]
        internal UnityEvent<string> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }

    [System.Serializable]
    internal class EventDataBool : EventDataBase
    {
        [SerializeField]
        internal UnityEvent<bool> unityEvent;
        public override UnityEventBase UnityEvent => unityEvent;
    }
}
