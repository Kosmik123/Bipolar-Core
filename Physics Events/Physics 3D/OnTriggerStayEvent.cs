using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerStayEvent : PhysicsEvent<Collider>
    {
        private void OnTriggerStay(Collider collision) => Invoke(collision);
    }
}
