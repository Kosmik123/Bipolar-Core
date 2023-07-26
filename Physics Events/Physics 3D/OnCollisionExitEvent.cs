using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnCollisionExitEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionExit(Collision collision) => Invoke(collision);
    }
}
