using UnityEngine;

namespace Bipolar.Pooling
{
    public class ComponentPool<T> : UnityObjectPool<T> where T : Component
    { }
}
