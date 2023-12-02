using System.Collections.Generic;
using UnityEngine;

namespace Bipolar
{
    public static class Extensions
    {
        public static T GetRandom<T>(this IReadOnlyList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)];
        }

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