using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionEnterEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionEnter(Collision collision) => Invoke(collision);
    }
}
