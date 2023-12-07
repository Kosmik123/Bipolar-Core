using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.PhysicsEvents
{
    public abstract class PhysicsEventBase<T> : MonoBehaviour where T : class
    {
        public delegate void PhysicsEventHandler(T collision);

        public event PhysicsEventHandler OnEvent;

        [SerializeField]
        private UnityEvent onEvent = new UnityEvent();
        
        public void Clear()
        {
            OnEvent = null;
        }

        protected void Invoke(T data)
        {
            onEvent.Invoke();
            OnEvent?.Invoke(data);
        }
    }

    public abstract class Physics2DEvent<T> : PhysicsEventBase<T> where T : class
    { }

    public abstract class PhysicsEvent<T> : PhysicsEventBase<T> where T : class
    { }
}
