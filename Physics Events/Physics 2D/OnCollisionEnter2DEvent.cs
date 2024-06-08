using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionEnter2DEvent : Collision2DEvent
    {
        private void OnCollisionEnter2D(Collision2D collision) => TryInvokeEvent(collision);
    }
}
