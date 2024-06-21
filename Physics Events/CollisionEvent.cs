using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.PhysicsEvents
{
    public abstract class PhysicsEventBase<T> : MonoBehaviour where T : class
    {
        public delegate void PhysicsEventHandler(T collision);

        public event PhysicsEventHandler OnEvent;

        [SerializeField]
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.Tag]
#endif
        [Tooltip("Specify tags to check. If empty: all tags will trigger the event")]
        private string[] detectedTags;

        [SerializeField]
        private UnityEvent onEvent = new UnityEvent();
        
        public void Clear()
        {
            OnEvent = null;
        }

        protected void InvokeEvents(T data)
        {
            onEvent.Invoke();
            OnEvent?.Invoke(data);
        }

        protected bool CompareTag(GameObject other)
        {
            if (detectedTags == null || detectedTags.Length <= 0)
                return true;

            foreach (var checkedTag in detectedTags)
                if (other.CompareTag(checkedTag))
                    return true;

            return false;
        }

        protected abstract void TryInvokeEvent(T data);
    }

    public abstract class TriggerEvent<T> : PhysicsEventBase<T> where T : Component
    {
        protected override void TryInvokeEvent(T collider)
        {
            if (CompareTag(collider.gameObject))
                InvokeEvents(collider);
        }
    }

    public abstract class CollisionEvent<T> : PhysicsEventBase<T> where T : class
    { }

    public abstract class Collision2DEvent : CollisionEvent<Collision2D>
    {
        protected override void TryInvokeEvent(Collision2D collision)
        {
            if (CompareTag(collision.gameObject))
                InvokeEvents(collision);
        }
    }

    public abstract class Collision3DEvent : CollisionEvent<Collision>
    {
        protected override void TryInvokeEvent(Collision collision)
        {
            if (CompareTag(collision.gameObject))
                InvokeEvents(collision);
        }
    }
}
