using System.Collections.Generic;
using UnityEngine;

namespace Bipolar
{
	public static class CollectionExtensions
	{
		public static T GetRandom<T>(this IReadOnlyList<T> collection)
		{
			return collection[Random.Range(0, collection.Count)];
		}
	}

	public static class ComponentExtensions
	{
		public static T GetRequired<T>(this MonoBehaviour owner, ref T component) where T : Component
		{
			if (component == null)
				component = owner.GetComponent<T>();
			return component;
		}
	}

	public static class TransformExtensions
	{
		public static void LookHorizontallyAt(this Transform transform, Transform target)
		{
			LookHorizontallyAt(transform, target.position);
		}

		public static void LookHorizontallyAt(this Transform transform, Vector3 target)
		{
			target.y = transform.position.y;
			transform.LookAt(target);
		}
	}
}
