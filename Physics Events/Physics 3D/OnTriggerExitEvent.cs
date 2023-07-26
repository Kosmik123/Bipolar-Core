using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnTriggerExitEvent : PhysicsEvent<Collider>
    {
        private void OnTriggerExit(Collider collision) => Invoke(collision);
    }
}
