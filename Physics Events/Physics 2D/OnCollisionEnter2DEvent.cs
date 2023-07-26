using UnityEngine;

namespace Bipolar.PhysicsEvents
{
    public class OnCollisionEnter2DEvent : Physics2DEvent<Collision2D>
    {
        private void OnCollisionEnter2D(Collision2D collision) => Invoke(collision);
    }
}
