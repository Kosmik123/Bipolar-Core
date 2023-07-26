using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerEnterEvent : PhysicsEvent<Collider>
    {
        private void OnTriggerEnter(Collider collision) => Invoke(collision);
    }
}
