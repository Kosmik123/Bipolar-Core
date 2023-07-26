using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionStay2DEvent : Physics2DEvent<Collision2D>
    {
        private void OnCollisionStay2D(Collision2D collision) => Invoke(collision);
    }
}
