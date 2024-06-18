using UnityEngine;

namespace Bipolar.Pooling
{
    public class ComponentPool<T> : PoolBase<T> where T : Component
    { }
}
