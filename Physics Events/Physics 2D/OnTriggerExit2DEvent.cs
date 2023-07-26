using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerExit2DEvent : Physics2DEvent<Collider2D>
    {
        private void OnTriggerExit2D(Collider2D collider) => Invoke(collider);
    }
}
