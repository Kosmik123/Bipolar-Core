using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerExitEvent : TriggerEvent<Collider>
    {
        private void OnTriggerExit(Collider collision) => TryInvokeEvent(collision);
    }
}
