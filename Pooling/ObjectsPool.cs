using System.Collections.Generic;

namespace Bipolar.Pooling
{
	public static class ObjectsPool<T>
		where T : new()
	{
		private readonly static Stack<T> pool = new Stack<T>();

		public static T Get() => pool.Count > 0 
			? pool.Pop() 
			: new T();

		public static void Release(T @object) => pool.Push(@object);
	}
}
