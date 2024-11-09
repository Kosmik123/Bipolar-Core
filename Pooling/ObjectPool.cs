using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Pooling
{
    public interface IObjectPool
    {
        int Count { get; }
        Object Get();
        void Release(Object @object);
    }

    public interface IObjectPool<T> : IObjectPool
        where T : Object
    {
        new T Get();
        void Release(T @object);
    }

    public class ObjectPool : ObjectPool<Object>
    { }

    public abstract class ObjectPool<T> : MonoBehaviour, IObjectPool<T>
        where T : Object
    {
        [SerializeField]
        protected T prototype;
        public T Prototype
        {
            get => prototype;
            set => prototype = value;
        }

        private readonly Stack<T> pool = new Stack<T>();

        public int Count => pool.Count;

        public T Get() => TryGetFromPool(out var @object)
            ? @object
            : Instantiate(prototype);

        private bool TryGetFromPool(out T @object)
        {
            @object = null;
            while (@object == null && pool.Count > 0)
                @object = pool.Pop();

            return @object != null;
        }

        public void Release(T pooledObject)
        {
            pool.Push(pooledObject);
        }

        public void Release(Object @object) => Release(@object);
        Object IObjectPool.Get() => Get();
    }
}
