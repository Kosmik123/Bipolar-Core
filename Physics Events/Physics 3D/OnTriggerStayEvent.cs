using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerStayEvent : TriggerEvent<Collider>
    {
        private void OnTriggerStay(Collider collision) => TryInvokeEvent(collision);
    }
}
