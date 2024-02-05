using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Pooling
{
    public abstract class PoolBase<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        protected T prototype;

        private readonly Stack<T> pool = new Stack<T>();

        public int Count => pool.Count;

        public T Get()
        {
            if (pool.Count == 0)
                return Instantiate(prototype);
            else
                return pool.Pop();
        }

        public void Release(T pooledObject)
        {
            pool.Push(pooledObject);
        }
    }
}
