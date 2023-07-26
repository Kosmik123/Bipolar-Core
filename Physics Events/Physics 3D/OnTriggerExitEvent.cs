using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerExitEvent : PhysicsEvent<Collider>
    {
        private void OnTriggerExit(Collider collision) => Invoke(collision);
    }
}
