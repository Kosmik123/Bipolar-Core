using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionEnterEvent : Collision3DEvent
    {
        private void OnCollisionEnter(Collision collision) => TryInvokeEvent(collision);
    }
}
