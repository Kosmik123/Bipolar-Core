using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.Subcomponents
{
	internal interface ISubcomponentList 
	{
		int Count { get; }
		void Update();
	}

	[System.Serializable]
	public class SubcomponentList<TComponent> : ISubcomponentList, IList<TComponent>, IReadOnlyList<TComponent>
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
			SubcomponentListUpdater.Instance.AddList(this);
			foreach (var subcomponent in items.OfType<IEnableCallbackReceiver>())
				subcomponent.OnEnable();
		}

		public void Disable()
		{
			foreach (var subcomponent in items.OfType<IDisableCallbackReceiver>())
				subcomponent.OnDisable();
		}

	    void ISubcomponentList.Update()
		{
			for (int i = 0; i < items.Count; i++)
			{

			}
		}
	}

	internal class SubcomponentListUpdater : MonoBehaviour
	{
		private static SubcomponentListUpdater instance;
		public static SubcomponentListUpdater Instance
		{
			get
			{
				if (instance = null)
					instance = new GameObject(string.Empty).AddComponent<SubcomponentListUpdater>();
				return instance;
			}
		}

		private List<ISubcomponentList> lists = new List<ISubcomponentList>();

		private void Awake()
		{
			if (instance != this)
				Destroy(gameObject);
		}

		public void AddList(ISubcomponentList list)
		{
			lists.Add(list);
		}

		private void Update()
		{
			for (int i = 0; i < lists.Count; i++)
			{
				lists[i].Update();
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
				instance = null;
		}
	}
}
