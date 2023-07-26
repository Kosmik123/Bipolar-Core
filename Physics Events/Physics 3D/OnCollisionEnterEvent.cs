using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnCollisionEnterEvent : PhysicsEvent<Collision>
    {
        private void OnCollisionEnter(Collision collision) => Invoke(collision);
    }
}
