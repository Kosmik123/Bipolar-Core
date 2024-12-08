using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.PhysicsEvents
{
	public abstract class PhysicsEventBase<T> : MonoBehaviour 
        where T : class
    {
        public delegate void PhysicsEventHandler(T collision);

        public event PhysicsEventHandler OnHappened;

#if NAUGHTY_ATTRIBUTES
		[NaughtyAttributes.Tag]
#endif
        [SerializeField]
        [Tooltip("Specify tags to check. If empty: all tags will trigger the event")]
        private string[] detectedTags;

        [SerializeField]
        private UnityEvent onEventHappen = new UnityEvent();

        public void Clear()
        {
            onEventHappen.RemoveAllListeners();
			OnHappened = null;
        }

		protected abstract GameObject GetGameObject(T data);

		protected void InvokeEvents(T data)
        {
            onEventHappen.Invoke();
            OnHappened?.Invoke(data);
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


        protected void TryInvokeEvent(T data)
        {
            if (CompareTag(GetGameObject(data)))
                InvokeEvents(data);
        }
    }

    public abstract class TriggerEvent<T> : PhysicsEventBase<T>
        where T : Component
    {
		protected override GameObject GetGameObject(T data) => data.gameObject;
    }

    public abstract class CollisionEvent<T> : PhysicsEventBase<T>
        where T : class
    { }

    public abstract class Collision2DEvent : CollisionEvent<Collision2D>
    {
		protected override GameObject GetGameObject(Collision2D data) => data.gameObject;
	}

    public abstract class Collision3DEvent : CollisionEvent<Collision>
    {
		protected override GameObject GetGameObject(Collision data) => data.gameObject;
	}
}
