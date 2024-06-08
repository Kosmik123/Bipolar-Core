using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionStayEvent : Collision3DEvent
    {
        private void OnCollisionStay(Collision collision) => TryInvokeEvent(collision);
    }
}
