using System;
using UnityEngine;
using UnityEngine.Windows;

namespace Bipolar
{
    [Serializable]
    public struct SerializedGuid : IComparable, IComparable<SerializedGuid>, IEquatable<SerializedGuid>, IFormattable
    {
        [SerializeField] private int a; 
        [SerializeField] private short b; 
        [SerializeField] private short c; 
        [SerializeField] private byte d; 
        [SerializeField] private byte e; 
        [SerializeField] private byte f; 
        [SerializeField] private byte g; 
        [SerializeField] private byte h; 
        [SerializeField] private byte i; 
        [SerializeField] private byte j;
        [SerializeField] private byte k;

        public static implicit operator Guid(SerializedGuid serializedGuid) => serializedGuid.ToGuid();
        public static explicit operator SerializedGuid(Guid guid) => new SerializedGuid(guid);
        public static SerializedGuid New() => new SerializedGuid(Guid.NewGuid());
        public int CompareTo(SerializedGuid other) => ToGuid().CompareTo(other.ToGuid());
        public int CompareTo(object obj) => ToGuid().CompareTo(obj);
        public readonly bool Equals(SerializedGuid other) => other.ToGuid().Equals(other.ToGuid());
        public string ToString(string format, IFormatProvider formatProvider) => ToGuid().ToString(format, formatProvider);
        public Guid ToGuid() => new Guid(a, b, c, d, e, f, g, h, i, j, k);
    
        private SerializedGuid(Guid input)
        {
            input.GetBackingValues(out a, out b, out c, out d, out e, out f, out g, out h, out i, out j, out k);
        }
    }


    public static class GuidExtension
    {
        public static void GetBackingValues(this Guid guid, out int a, out short b, out short c, out byte d,
            out byte e, out byte f, out byte g, out byte h, out byte i, out byte j, out byte k)
        {
#if UNITY_2022_2_OR_NEWER
            Span<byte> guidBytes = stackalloc byte[16];
            guid.TryWriteBytes(guidBytes);
            ReadOnlySpan<byte> bytes = guidBytes;
            a = BitConverter.ToInt32(bytes);
            b = BitConverter.ToInt16(bytes[4..]);
            c = BitConverter.ToInt16(bytes[6..]);
#else
            var bytes = guid.ToByteArray();
            a = BitConverter.ToInt32(bytes, 0);
            b = BitConverter.ToInt16(bytes, 4);
            c = BitConverter.ToInt16(bytes, 6);
#endif
            d = bytes[8];
            e = bytes[9];
            f = bytes[10];
            g = bytes[11];
            h = bytes[12];
            i = bytes[13];
            j = bytes[14];
            k = bytes[15];
        }
    }
}

