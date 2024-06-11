using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Prototyping
{
    public abstract class Event : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onEvent = new UnityEvent();

        protected void InvokeEvent()
        {
            onEvent.Invoke();
        }
    }

    public class StartEvent : Event
    {
        private void Start() => InvokeEvent();
    }

}
