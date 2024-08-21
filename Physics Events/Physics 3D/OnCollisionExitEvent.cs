using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionExitEvent : Collision3DEvent
    {
        private void OnCollisionExit(Collision collision) => TryInvokeEvent(collision);
    }
}
