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

    }

}