using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerStay2DEvent : Physics2DEvent<Collider2D>
    {
        private void OnTriggerStay2D(Collider2D collider) => Invoke(collider);
    }
}
