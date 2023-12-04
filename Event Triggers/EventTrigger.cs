using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public abstract class EventTrigger<T> : MonoBehaviour where T : BaseEventData
    {
        public delegate void EventTriggerHandler(T data);

        public event EventTriggerHandler OnEventTriggered;

        [SerializeField]
        private UnityEvent<T> onEventTrigger;

        protected void Execute(T data)
        {
            onEventTrigger.Invoke(data);
            OnEventTriggered?.Invoke(data);
        }
    }

    public abstract class EventTrigger : EventTrigger<BaseEventData>
    { }

    public abstract class PointerEventTrigger : EventTrigger<PointerEventData>
    { }

    public abstract class AxisEventTrigger : EventTrigger<AxisEventData> 
    { }
}
