using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Pooling
{
    public interface IPool
    {
        int Count { get; }
    }

    public interface IPool<T> : IPool
    {
        T Get();
        void Release(T @object);
    }

	public abstract class Pool<T> : MonoBehaviour, IPool<T>
	{
		public delegate T CreateFunc(T prototype);

		private readonly Stack<T> pool = new Stack<T>();
		public int Count => pool.Count;

		public System.Action<T> OnSpawnAction { get; set; }
		public CreateFunc CreateFunction { get; set; }

		public T Get() => TryGetFromPool(out var @object)
			? @object
			: SpawnNew();

		protected abstract T SpawnNew();

		private bool TryGetFromPool(out T @object)
		{
			@object = default;
			while (@object == null && pool.Count > 0)
				@object = pool.Pop();

			return @object != null;
		}

		public void Release(T pooledObject)
		{
			pool.Push(pooledObject);
		}
	}

	public abstract class ObjectPool<T> : Pool<T>
		where T : Object
	{
		[SerializeField]
		protected T prototype;
		public T Prototype
		{
			get => prototype;
			set => prototype = value;
		}

		protected override T SpawnNew()
		{
			T instance = CreateFunction != null ? CreateFunction(prototype) : Instantiate(prototype);
			OnSpawnAction?.Invoke(instance);
			return instance;
		}
	}

	public class ObjectPool : ObjectPool<Object>
	{ }
}
