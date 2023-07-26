using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnCollisionStayEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionStay(Collision collision) => Invoke(collision);
    }
}
