using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerEnterEvent : TriggerEvent<Collider>
    {
        private void OnTriggerEnter(Collider collision) => TryInvokeEvent(collision);
    }
}
