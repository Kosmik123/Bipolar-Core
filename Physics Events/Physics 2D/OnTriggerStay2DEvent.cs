using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerStay2DEvent : TriggerEvent<Collider2D>
    {
        private void OnTriggerStay2D(Collider2D collider) => TryInvokeEvent(collider);
    }
}
