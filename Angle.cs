using System;
using UnityEngine;

namespace Bipolar
{
    public struct Angle : IComparable<Angle>, IEquatable<Angle>
    {
        public float Value { get; private set; }

        private Angle(float angle)
        {
            Value = angle;
        }

        public static Angle operator +(Angle angle, float addedValue) 
        {
            return new Angle(angle.Value + addedValue);
        }

        public static Angle FromDegrees(float degrees)
        {
            float angle = degrees % 360f;
            if (angle < 0)
                angle += 360;
            angle /= 180;
            if (angle > 1) 
                angle -= 2;

            return new Angle(angle);
        }

        public static Angle FromRadians(float radians)
        {
            float angle = radians % (2 * Mathf.PI);
            if (angle < 0)
                angle += 2 * Mathf.PI;
            angle /= Mathf.PI;
            if (angle > 1)
                angle -= 2;

            return new Angle(angle);
        }

        public float ToDegrees() => 180 * Value;
        public float ToRadians() => Mathf.PI * Value;

        public bool Equals(Angle other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Angle other)
        {
            return Value.CompareTo(other.Value);
        }

        public static bool operator <(Angle left, Angle right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Angle left, Angle right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Angle left, Angle right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Angle left, Angle right)
        {
            return left.CompareTo(right) >= 0;
        }

        public override bool Equals(object other)
        {
            if (!(other is Angle))
                return false;

            return Equals((Angle)other);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => ToDegrees().ToString();

        public static bool operator ==(Angle lhs, Angle rhs) => lhs.Equals(rhs);
        public static bool operator !=(Angle lhs, Angle rhs) => !lhs.Equals(rhs);

        public static explicit operator Angle(float angle) => new Angle(angle);
        public static explicit operator float(Angle angle) => angle.Value;
    }
}
