using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionExitEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionExit(Collision collision) => Invoke(collision);
    }
}
