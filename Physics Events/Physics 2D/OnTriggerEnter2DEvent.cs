using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerEnter2DEvent : TriggerEvent<Collider2D>
    {
        private void OnTriggerEnter2D(Collider2D collider) => TryInvokeEvent(collider);
    }
}
