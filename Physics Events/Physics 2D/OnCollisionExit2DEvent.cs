using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public sealed class OnCollisionExit2DEvent : Physics2DEvent<Collision2D>
    {
        private void OnCollisionExit2D(Collision2D collision) => Invoke(collision);
    }
}
