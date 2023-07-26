using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnCollisionStay2DEvent : Physics2DEvent<Collision2D>
    {
        private void OnCollisionStay2D(Collision2D collision) => Invoke(collision);
    }
}
