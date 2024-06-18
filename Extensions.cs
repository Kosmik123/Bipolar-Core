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

	public static class ColorUtility
	{
		public static Color32 FromDrawingColor(System.Drawing.Color source)
		{
			return new Color32(source.R, source.G, source.B, source.A);
		}
	}


}