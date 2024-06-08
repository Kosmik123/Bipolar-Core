using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerExit2DEvent : TriggerEvent<Collider2D>
    {
        private void OnTriggerExit2D(Collider2D collider) => TryInvokeEvent(collider);
    }
}
