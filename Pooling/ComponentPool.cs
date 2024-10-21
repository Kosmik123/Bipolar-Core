using UnityEngine;

namespace Bipolar.Pooling
{
    public class ComponentPool<T> : ObjectPool<T> where T : Component
    { }
}
