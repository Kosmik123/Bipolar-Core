using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.PhysicsEvents
{
    public abstract class PhysicsEventBase<T> : MonoBehaviour where T : class
    {
        public event System.Action<T> OnEvent;

        [SerializeField]
        private UnityEvent onEvent = new UnityEvent();

        public void AddListenter(System.Action<T> action)
        {
            OnEvent += action;
        }

        public void RemoveListenter(System.Action<T> action)
        {
            OnEvent -= action;
        }
        
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

    public abstract class Physics2DEvent<T> : PhysicsEventBase<T>  where T : class
    { }

    public abstract class PhysicsEvent<T> : PhysicsEventBase<T> where T : class
    { }
}
