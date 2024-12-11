using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.Subcomponents
{
	public interface ICompoundBehavior 
	{
		int SubcomponentsCount { get; }
		IReadOnlyList<ISubcomponent> Subcomponents { get; }
		Type SubcomponentsType { get; }
	}

	public class CompoundBehavior<TComponent> : MonoBehaviour, ICompoundBehavior
		where TComponent : ISubcomponent
	{
		[SerializeReference] // it must be serialize reference since every component will be different type
		[HideInInspector]
		internal protected List<TComponent> subcomponents = new List<TComponent>();

		protected readonly List<IUpdatable> updatableSubcomponents = new List<IUpdatable>();

		public int SubcomponentsCount => subcomponents.Count;
		public IReadOnlyList<TComponent> Subcomponents => subcomponents;
		IReadOnlyList<ISubcomponent> ICompoundBehavior.Subcomponents => (IReadOnlyList<ISubcomponent>)subcomponents;

		public Type SubcomponentsType => typeof(TComponent);

		public TComponent AddSubcomponent<T>()
			where T : TComponent, new()
		{
			var component = new T();
			AddSubcomponent(component);
			return component;
		}

		public void AddSubcomponent(TComponent component)
		{
			subcomponents.Add(component);
			TryAddToSubList(updatableSubcomponents, component);
		}

		private static bool TryAddToSubList<T>(List<T> list, TComponent component)
			where T : ISubcomponent
		{
			if (component is T subType)
			{
				list.Add(subType);
				return true;
			}
			return false;
		}

		public void RemoveSubcomponent(TComponent component)
		{
			subcomponents.Remove(component);
			RemoveFromSubList(updatableSubcomponents, component);
		}

		private static void RemoveFromSubList<T>(List<T> list, TComponent component)
			where T : ISubcomponent
		{
			if (component is T subType)
				list.Remove(subType);
		}

		public bool TryGetSubcomponent<T>(out T component)
			where T : TComponent
		{
			component = default;
			for (int i = 0; i < subcomponents.Count; i++)
			{
				if (subcomponents[i] is T correctType)
				{
					component = correctType;
					return true;
				}
			}
			return false;
		}
		
		protected virtual void Awake()
		{
			updatableSubcomponents.Clear();
			foreach (var subcomponent in subcomponents)
			{
				TryAddToSubList(updatableSubcomponents, subcomponent);
			}
		}

		protected virtual void OnEnable()
		{
			foreach (var subcomponent in subcomponents.OfType<IEnableCallbackReceiver>())
				subcomponent.OnEnable();
		}

		protected virtual void Update()
		{
			for (int i = 0; i < updatableSubcomponents.Count; i++)
			{
				var updatable = updatableSubcomponents[i];
				if (!(updatable is SubBehavior subBehavior) || subBehavior.IsEnabled)
					updatable.Update();
			}
		}

		protected virtual void OnDisable()
		{
			foreach (var subcomponent in subcomponents.OfType<IDisableCallbackReceiver>())
				subcomponent.OnDisable();
		}
	}
}
