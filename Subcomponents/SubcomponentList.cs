using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.Subcomponents
{
	[System.Serializable]
	public class SubcomponentList<TComponent> : IList<TComponent>, IReadOnlyList<TComponent>
		where TComponent : ISubcomponent, new()
	{
		[SerializeField] // soon it could be serialize reference
		protected List<TComponent> items = new List<TComponent>();

		public TComponent this[int index]
		{
			get => items[index];
			set => items[index] = value;
		}

		public int Count => items.Count;
		public bool IsReadOnly => ((IList<TComponent>)items).IsReadOnly;
		public void Add(TComponent item) => items.Add(item);
		public void Clear() => items.Clear();
		public bool Contains(TComponent item) => items.Contains(item);
		public void CopyTo(TComponent[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
		public IEnumerator<TComponent> GetEnumerator() => items.GetEnumerator();
		public int IndexOf(TComponent item) => items.IndexOf(item);
		public void Insert(int index, TComponent item) => items.Insert(index, item);
		public bool Remove(TComponent item) => items.Remove(item);
		public void RemoveAt(int index) => items.RemoveAt(index);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Enable()
		{
			foreach (var subcomponent in items.OfType<IEnableCallbackReceiver>())
				subcomponent.OnEnable();
		}

		public void Disable()
		{
			foreach (var subcomponent in items.OfType<IDisableCallbackReceiver>())
				subcomponent.OnDisable();
		}
	}
}
