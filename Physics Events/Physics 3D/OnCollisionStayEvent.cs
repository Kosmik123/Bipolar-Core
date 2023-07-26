using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionStayEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionStay(Collision collision) => Invoke(collision);
    }
}
