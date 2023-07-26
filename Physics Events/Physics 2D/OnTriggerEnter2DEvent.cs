using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnTriggerEnter2DEvent : Physics2DEvent<Collider2D>
    {
        private void OnTriggerEnter2D(Collider2D collider) => Invoke(collider);
    }
}
