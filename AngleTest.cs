#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif
using UnityEngine;

namespace Bipolar
{
    public class AngleTest : MonoBehaviour
    {
        public float radians;

        public Angle angle;

#if NAUGHTY_ATTRIBUTES
        [ReadOnly]
#endif
        public float radiansTransformed;

        private void OnValidate()
        {
            angle = Angle.FromRadians(radians);
            radiansTransformed = angle.ToRadians();
        }
    }

    public class Auto<T> where T : Component
    {
        private MonoBehaviour owner;

        private T value;
        public T Value
        {
            get
            {
                if (value == null)
                    value = owner.GetComponent<T>();
                return value;
            }
        }

        public Auto(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        public static implicit operator T(Auto<T> auto) => auto.Value;
    }
}
